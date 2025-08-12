using MoneyTrack.Application.Models.Category;

namespace MoneyTrack.Application.Models.Transaction;

public class AITransactionDto
{
    public string Description { get; set; }

    public double Amount { get; set; }

    public DateTime ExpenseDate { get; set; }
    
    public string CategoryCode { get; set; }
}