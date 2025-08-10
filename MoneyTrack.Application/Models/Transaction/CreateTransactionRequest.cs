namespace MoneyTrack.Application.Models.Transaction;

public class CreateTransactionRequest
{
    public string Description { get; set; }

    public double Amount { get; set; }

    public DateTime ExpenseDate { get; set; }

    public Guid UserId { get; set; }

    public string CategoryCode { get; set; }
}