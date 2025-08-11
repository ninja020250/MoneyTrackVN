namespace MoneyTrack.Application.Models.Transaction;

public class UpdateTransactionRequest
{
    public Guid Id { get; set; }
    
    public string Description { get; set; }

    public double Amount { get; set; }

    public DateTime ExpenseDate { get; set; }

    public string CategoryCode { get; set; }
}