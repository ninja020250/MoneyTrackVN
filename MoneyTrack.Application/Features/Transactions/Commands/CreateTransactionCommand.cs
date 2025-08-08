using System.Transactions;
using MediatR;

namespace MoneyTrack.Application.Features.Transactions.Commands;

public class CreateTransactionCommand : IRequest<CreateTransactionCommandResponse>
{
    public string Description { get; set; }

    public double Amount { get; set; }

    public DateTime ExpenseDate { get; set; }

    public Guid UserId { get; set; }


    public string CategoryCode { get; set; }
}