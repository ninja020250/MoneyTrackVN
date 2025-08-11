using MediatR;
using MoneyTrack.Application.Features.Transactions.Queries;
using MoneyTrack.Application.Models.Transaction;

namespace MoneyTrack.Application.Features.Transactions.Commands;

public class BulkUpdateTransactionCommand:  IRequest<GetTransactionResponse>
{
    public List<UpdateTransactionRequest> Transactions { get; set; }
}