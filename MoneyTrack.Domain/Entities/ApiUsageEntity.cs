namespace MoneyTrack.Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class ApiUsageEntity: AuditableEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    [MaxLength(255)]
    public string ApiName { get; set; }

    [Required]
    public DateTime CallDate { get; set; }

    [Required]
    public int CallCount { get; set; }
}