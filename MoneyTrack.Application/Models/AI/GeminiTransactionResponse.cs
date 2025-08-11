using System.Text.Json.Serialization;

namespace MoneyTrack.Application.Models.AI;

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