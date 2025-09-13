using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Application.Models.AI;
using MoneyTrack.Application.Models.Transaction;
using MoneyTrack.Domain.Entities;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MoneyTrack.Infrastructure.AI;

public class GeminiService(
    HttpClient httpClient,
    IOptions<GeminiSettings> settings,
    ILogger<GeminiService> logger
)
    : IGeminiService
{
    private readonly GeminiSettings _settings = settings.Value;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };


    public async Task<AITransactionDto> ParseTransactionAsync(
        string message,
        string language,
        string currencyUnit,
        IReadOnlyList<TransactionCategoryEntity> categories)
    {
        try
        {
            logger.LogInformation("Parsing transaction message: {Message}", message);

            var prompt = BuildTransactionPrompt(message, language, currencyUnit, categories);
            var geminiRequest = CreateGeminiRequest(prompt);

            var response = await CallGeminiApiAsync(geminiRequest);
            var parsedTransaction = ParseGeminiResponse(response);

            return new AITransactionDto()
            {
                Description = parsedTransaction.Description,
                Amount = parsedTransaction.Amount,
                ExpenseDate = parsedTransaction.ExpenseDate,
                CategoryCode = parsedTransaction.CategoryCode
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error parsing transaction message: {Message}", message);
            throw new InvalidOperationException("Failed to parse transaction message using AI", ex);
        }
    }


    public string BuildTransactionPrompt(string message, string language, string currencyUnit,
        IReadOnlyList<TransactionCategoryEntity> categories)
    {
        return $@"
            Parse the following transaction message and extract the transaction details.
            Return a JSON object with the following structure:
            {{
                ""description"": ""brief description of the transaction"",
                ""amount"": numeric_amount,
                ""expenseDate"": ""ISO_date_string"",
                ""categoryCode"": ""category_code""
            }}
            
            Available category codes: {JsonConvert.SerializeObject(categories)}
            
            Common Rules:
            - If the date is not specified, use today's date is {DateTime.UtcNow.ToString()}.
            - If the category cannot be null and should be one of code of list category codes.
            - Amount should be positive number
            - Description should be concise but descriptive
            - Description should follow language as the user's message
            - Message referred language: {language}
            - currency unit is {currencyUnit}

            Vietnamese rules: 
            - If the date is not specified, use today's date is {DateTime.UtcNow.ToString()}.
            - If message say ""Hôm qua"", the ExpenseDate should be the yesterday date {DateTime.UtcNow.AddDays(-1).ToString()}, please check the current realtime.
            - Some currency unit is special: 
                * ""củ khoai"" =  1000000vnd
                * ""1 chai"" = 100000vnd
                * ""1 lốp"" = 100000vnd
                * ""1 tỏi"" = 1000000000vnd

            
            Message to parse: {message}
            ";
    }

    private object CreateGeminiRequest(string prompt)
    {
        return new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            },
            generationConfig = new
            {
                response_mime_type = "application/json",
                response_schema = new
                {
                    type = "object",
                    properties = new
                    {
                        description = new { type = "string" },
                        amount = new { type = "number" },
                        expenseDate = new { type = "string" },
                        categoryCode = new { type = "string" }
                    },
                    required = new[] { "description", "amount", "expenseDate", "categoryCode" }
                },
                temperature = _settings.Temperature,
                maxOutputTokens = _settings.MaxTokens
            }
        };
    }

    private async Task<string> CallGeminiApiAsync(object request)
    {
        var json = JsonSerializer.Serialize(request, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var url = _settings.ToString();

        httpClient.Timeout = TimeSpan.FromSeconds(_settings.TimeoutSeconds);

        var response = await httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            logger.LogError("Gemini API error: {StatusCode} - {Content}", response.StatusCode, errorContent);
            throw new HttpRequestException($"Gemini API call failed: {response.StatusCode}");
        }

        return await response.Content.ReadAsStringAsync();
    }

    private GeminiTransactionResponse ParseGeminiResponse(string response)
    {
        try
        {
            var geminiResponse = JsonSerializer.Deserialize<dynamic>(response);
            var candidatesElement = ((JsonElement)geminiResponse).GetProperty("candidates");
            var firstCandidate = candidatesElement[0];
            var contentElement = firstCandidate.GetProperty("content");
            var partsElement = contentElement.GetProperty("parts");
            var textElement = partsElement[0].GetProperty("text");
            var transactionJson = textElement.GetString();

            var transaction = JsonSerializer.Deserialize<GeminiTransactionResponse>(transactionJson!, _jsonOptions);

            // Validate and set defaults
            if (string.IsNullOrEmpty(transaction.Description))
                transaction.Description = "AI-parsed transaction";

            if (transaction.Amount <= 0)
                throw new InvalidOperationException("Invalid amount parsed from message");

            if (string.IsNullOrEmpty(transaction.CategoryCode))
                transaction.CategoryCode = "OTHER";

            if (transaction.ExpenseDate != null)
                transaction.ExpenseDate = transaction.ExpenseDate.ToUniversalTime();

            if (transaction.ExpenseDate == null)
                transaction.ExpenseDate = DateTime.UtcNow;

            return transaction;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error parsing Gemini response: {Response}", response);
            throw new InvalidOperationException("Failed to parse Gemini AI response", ex);
        }
    }
}