using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoneyTrack.Domain.Entities;

public class TransactionEntity: AuditableEntity
{
    [Key]
    public Guid Id { get; set; }

    public string? Description { get; set; }

    public double Amount { get; set; }
    
    public DateTime? ExpenseDate { get; set; }

    public Guid UserId { get; set; }

    [JsonIgnore] public UserEntity User { get; set; }

    public Guid CategoryId { get; set; }
    
    public TransactionCategoryEntity? Category { get; set; }
}