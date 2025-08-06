using System.Text.Json.Serialization;

namespace MoneyTrack.Domain.Entities;

public class TransactionCategoryEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    
    public string Code { get; set; }
    
    public string? Description { get; set; }
    
    [JsonIgnore] 
    public ICollection<TransactionEntity> Transactions { get; set; } = new List<TransactionEntity>();
}