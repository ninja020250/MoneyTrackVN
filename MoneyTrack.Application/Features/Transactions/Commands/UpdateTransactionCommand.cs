using MediatR;

namespace MoneyTrack.Application.Features.Transactions.Commands;

public class UpdateTransactionCommand : IRequest<CreateTransactionCommandResponse>
{
    public Guid Id { get; set; }
    
    public string Description { get; set; }

    public double Amount { get; set; }

    public DateTime ExpenseDate { get; set; }

    public string CategoryCode { get; set; }
}