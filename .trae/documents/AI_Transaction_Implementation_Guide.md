# AI-Powered Transaction Creation - Implementation Guide

## 1. Implementation Overview

This guide provides step-by-step instructions for implementing the AI-powered transaction creation feature in the MoneyTrack application. The implementation follows Clean Architecture principles and integrates seamlessly with the existing codebase.

## 2. Prerequisites

* Google Gemini API key

* Existing MoneyTrack application setup

* .NET 8 SDK

* Understanding of MediatR pattern

## 3. Step-by-Step Implementation

### 3.1 Update ILLMService Interface

**File**: `MoneyTrack.Application/Contracts/Infrastructure/ILLMService.cs`

```csharp
using MoneyTrack.Application.Models.Transaction;

namespace MoneyTrack.Application.Contracts.Infrastructure;

public interface ILLMService
{
    public object ParseMessageToObject(string message);
}

// Specific interface for Gemini integration
public interface IGeminiLLMService : ILLMService
{
    Task<CreateTransactionRequest> ParseMessageToTransactionAsync(string message, Guid userId);
}
```

### 3.2 Create New Models

**File**: `MoneyTrack.Application/Models/Transaction/CreateTransactionFromMessageRequest.cs`

```csharp
namespace MoneyTrack.Application.Models.Transaction;

public class CreateTransactionFromMessageRequest
{
    public string Message { get; set; } = string.Empty;
}
```

**File**: `MoneyTrack.Application/Models/Transaction/GeminiTransactionResponse.cs`

```csharp
using System.Text.Json.Serialization;

namespace MoneyTrack.Application.Models.Transaction;

public class GeminiTransactionResponse
{
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
    
    [JsonPropertyName("amount")]
    public double Amount { get; set; }
    
    [JsonPropertyName("expenseDate")]
    public DateTime ExpenseDate { get; set; }
    
    [JsonPropertyName("categoryCode")]
    public string CategoryCode { get; set; } = string.Empty;
}
```

### 3.3 Create Command and Handler

**File**: `MoneyTrack.Application/Features/Transactions/Commands/CreateTransactionFromMessage/CreateTransactionFromMessageCommand.cs`

```csharp
using MediatR;
using MoneyTrack.Application.Features.Transactions.Queries;
using MoneyTrack.Application.Models.Transaction;

namespace MoneyTrack.Application.Features.Transactions.Commands.CreateTransactionFromMessage;

public class CreateTransactionFromMessageCommand : CreateTransactionFromMessageRequest, IRequest<GetTransactionResponse>
{
    public Guid UserId { get; set; }
}
```

**File**: `MoneyTrack.Application/Features/Transactions/Commands/CreateTransactionFromMessage/CreateTransactionFromMessageCommandHandler.cs`

```csharp
using AutoMapper;
using MediatR;
using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Features.Transactions.Queries;
using MoneyTrack.Application.Models.Transaction;
using MoneyTrack.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace MoneyTrack.Application.Features.Transactions.Commands.CreateTransactionFromMessage;

public class CreateTransactionFromMessageCommandHandler : IRequestHandler<CreateTransactionFromMessageCommand, GetTransactionResponse>
{
    private readonly IGeminiLLMService _geminiService;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionCategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateTransactionFromMessageCommandHandler> _logger;

    public CreateTransactionFromMessageCommandHandler(
        IGeminiLLMService geminiService,
        ITransactionRepository transactionRepository,
        ITransactionCategoryRepository categoryRepository,
        IMapper mapper,
        ILogger<CreateTransactionFromMessageCommandHandler> logger)
    {
        _geminiService = geminiService;
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetTransactionResponse> Handle(CreateTransactionFromMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Processing AI transaction creation for user {UserId} with message: {Message}", 
                request.UserId, request.Message);

            // Parse message using Gemini AI
            var transactionRequest = await _geminiService.ParseMessageToTransactionAsync(request.Message, request.UserId);
            
            // Validate and get category
            var category = await _categoryRepository.GetByCodeAsync(transactionRequest.CategoryCode);
            if (category == null)
            {
                _logger.LogWarning("Category {CategoryCode} not found, using default", transactionRequest.CategoryCode);
                category = await _categoryRepository.GetByCodeAsync("OTHER");
            }

            // Create transaction entity
            var transaction = new TransactionEntity
            {
                Id = Guid.NewGuid(),
                Description = transactionRequest.Description,
                Amount = transactionRequest.Amount,
                ExpenseDate = transactionRequest.ExpenseDate,
                UserId = request.UserId,
                CategoryCode = category?.Code ?? "OTHER",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Save transaction
            var createdTransaction = await _transactionRepository.CreateAsync(transaction);
            
            // Map to DTO
            var transactionDto = _mapper.Map<GetTransactionDto>(createdTransaction);
            transactionDto.Category = _mapper.Map<GetCategoryDto>(category);

            _logger.LogInformation("Successfully created transaction {TransactionId} for user {UserId}", 
                createdTransaction.Id, request.UserId);

            return new GetTransactionResponse
            {
                Success = true,
                Message = "Transaction created successfully from AI parsing",
                Transactions = new List<GetTransactionDto> { transactionDto }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating transaction from message for user {UserId}", request.UserId);
            return new GetTransactionResponse
            {
                Success = false,
                Message = "Failed to create transaction from message. Please try again or create manually.",
                Transactions = new List<GetTransactionDto>()
            };
        }
    }
}
```

**File**: `MoneyTrack.Application/Features/Transactions/Commands/CreateTransactionFromMessage/CreateTransactionFromMessageCommandValidator.cs`

```csharp
using FluentValidation;

namespace MoneyTrack.Application.Features.Transactions.Commands.CreateTransactionFromMessage;

public class CreateTransactionFromMessageCommandValidator : AbstractValidator<CreateTransactionFromMessageCommand>
{
    public CreateTransactionFromMessageCommandValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required")
            .MinimumLength(5).WithMessage("Message must be at least 5 characters")
            .MaximumLength(500).WithMessage("Message cannot exceed 500 characters");
            
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");
    }
}
```

### 3.4 Create Gemini Service Implementation

**File**: `MoneyTrack.Infrastructure/AI/GeminiSettings.cs`

```csharp
namespace MoneyTrack.Infrastructure.AI;

public class GeminiSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1beta";
    public string Model { get; set; } = "gemini-1.5-flash";
    public int MaxTokens { get; set; } = 1000;
    public double Temperature { get; set; } = 0.1;
    public int TimeoutSeconds { get; set; } = 30;
}
```

**File**: `MoneyTrack.Infrastructure/AI/GeminiLLMService.cs`

```csharp
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Application.Models.Transaction;

namespace MoneyTrack.Infrastructure.AI;

public class GeminiLLMService : IGeminiLLMService
{
    private readonly HttpClient _httpClient;
    private readonly GeminiSettings _settings;
    private readonly ILogger<GeminiLLMService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public GeminiLLMService(
        HttpClient httpClient,
        IOptions<GeminiSettings> settings,
        ILogger<GeminiLLMService> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public CreateTransactionRequest ParseMessageToObject(string message)
    {
        return ParseMessageToTransactionAsync(message, Guid.Empty).GetAwaiter().GetResult();
    }

    public async Task<CreateTransactionRequest> ParseMessageToTransactionAsync(string message, Guid userId)
    {
        try
        {
            _logger.LogInformation("Parsing transaction message for user {UserId}: {Message}", userId, message);

            var prompt = BuildTransactionPrompt(message);
            var geminiRequest = CreateGeminiRequest(prompt);
            
            var response = await CallGeminiApiAsync(geminiRequest);
            var parsedTransaction = ParseGeminiResponse(response);
            
            return new CreateTransactionRequest
            {
                Description = parsedTransaction.Description,
                Amount = parsedTransaction.Amount,
                ExpenseDate = parsedTransaction.ExpenseDate,
                UserId = userId,
                CategoryCode = parsedTransaction.CategoryCode
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing transaction message: {Message}", message);
            throw new InvalidOperationException("Failed to parse transaction message using AI", ex);
        }
    }

    private string BuildTransactionPrompt(string message)
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
            
            Available category codes: FOOD, TRANSPORT, SHOPPING, ENTERTAINMENT, UTILITIES, HEALTHCARE, OTHER
            
            Rules:
            - If the date is not specified, use today's date
            - If the category cannot be determined, use 'OTHER'
            - Amount should be positive number
            - Description should be concise but descriptive
            
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
        
        var url = $"{_settings.BaseUrl}/models/{_settings.Model}:generateContent?key={_settings.ApiKey}";
        
        _httpClient.Timeout = TimeSpan.FromSeconds(_settings.TimeoutSeconds);
        
        var response = await _httpClient.PostAsync(url, content);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Gemini API error: {StatusCode} - {Content}", response.StatusCode, errorContent);
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
                
            return transaction;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing Gemini response: {Response}", response);
            throw new InvalidOperationException("Failed to parse Gemini AI response", ex);
        }
    }
}
```

### 3.5 Update Controller

**File**: `MoneyTrack.Api/Controllers/TransactionController.cs` (Add new endpoint)

```csharp
[HttpPost("ai-create")]
[Authorize] // Ensure user is authenticated
public async Task<ActionResult<GetTransactionResponse>> CreateTransactionFromMessage(
    [FromBody] CreateTransactionFromMessageRequest request)
{
    var userId = GetUserIdFromClaims(); // Extract from JWT claims
    
    var command = new CreateTransactionFromMessageCommand
    {
        Message = request.Message,
        UserId = userId
    };
    
    return await _mediator.Send(command);
}

private Guid GetUserIdFromClaims()
{
    var userIdClaim = User.FindFirst("userId")?.Value;
    if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
    {
        throw new UnauthorizedAccessException("Invalid user token");
    }
    return userId;
}
```

### 3.6 Configuration Updates

**File**: `MoneyTrack.Infrastructure/InfrastructureServiceRegistration.cs`

```csharp
// Add to ConfigureServices method
services.Configure<GeminiSettings>(configuration.GetSection("GeminiSettings"));

services.AddHttpClient<IGeminiLLMService, GeminiLLMService>(client =>
{
    client.DefaultRequestHeaders.Add("User-Agent", "MoneyTrack/1.0");
});

services.AddScoped<IGeminiLLMService, GeminiLLMService>();
```

**File**: `appsettings.json`

```json
{
  "GeminiSettings": {
    "ApiKey": "your-gemini-api-key-here",
    "BaseUrl": "https://generativelanguage.googleapis.com/v1beta",
    "Model": "gemini-1.5-flash",
    "MaxTokens": 1000,
    "Temperature": 0.1,
    "TimeoutSeconds": 30
  }
}
```

**File**: `appsettings.Development.json`

```json
{
  "GeminiSettings": {
    "ApiKey": "your-development-api-key"
  }
}
```

## 4. Testing Strategy

### 4.1 Unit Tests

```csharp
[Test]
public async Task Handle_ValidMessage_ReturnsSuccessResponse()
{
    // Arrange
    var mockGeminiService = new Mock<IGeminiLLMService>();
    var mockTransactionRepo = new Mock<ITransactionRepository>();
    var mockCategoryRepo = new Mock<ITransactionCategoryRepository>();
    
    // Setup mocks and test data
    
    // Act
    var result = await handler.Handle(command, CancellationToken.None);
    
    // Assert
    Assert.IsTrue(result.Success);
    Assert.IsNotEmpty(result.Transactions);
}
```

### 4.2 Integration Tests

```csharp
[Test]
public async Task CreateTransactionFromMessage_ValidRequest_ReturnsTransaction()
{
    // Test the full API endpoint with real Gemini integration
    var client = _factory.CreateClient();
    var request = new CreateTransactionFromMessageRequest
    {
        Message = "I spent $15 on coffee this morning"
    };
    
    var response = await client.PostAsJsonAsync("/api/transactions/ai-create", request);
    
    response.EnsureSuccessStatusCode();
    var result = await response.Content.ReadFromJsonAsync<GetTransactionResponse>();
    
    Assert.IsTrue(result.Success);
    Assert.AreEqual(1, result.Transactions.Count);
}
```

## 5. Deployment Considerations

1. **Environment Variables**: Store Gemini API key securely
2. **Rate Limiting**: Implement API rate limiting to prevent abuse
3. **Monitoring**: Add logging and metrics for AI API calls
4. **Error Handling**: Implement graceful fallbacks for AI service failures
5. **Cost Management**: Monitor Gemini API usage and costs

## 6. Future Enhancements

1. **Caching**: Cache common transaction patterns
2. **Learning**: Improve prompts based on user feedback
3. **Multi-language**: Support multiple languages for transaction parsing
4. **Batch Processing**: Support multiple transactions in one message
5. **Voice Integration**: Add voice-to-text for hands-free transaction creation

