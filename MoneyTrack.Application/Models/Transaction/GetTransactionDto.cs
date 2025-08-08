using MoneyTrack.Application.Models.Category;

namespace MoneyTrack.Application.Models.Transaction;

public class GetTransactionDto
{
    public Guid Id { get; set; }

    public string? Description { get; set; }

    public double Amount { get; set; }

    public DateTime? ExpenseDate { get; set; }

    public Guid UserId { get; set; }

    public GetCategoryDto? Category { get; set; }
}