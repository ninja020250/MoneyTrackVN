using MediatR;
using MoneyTrack.Application.Features.Transactions.Queries;
using MoneyTrack.Application.Models.Transaction;

namespace MoneyTrack.Application.Features.Transactions.Commands;

public class BulkUpdateTransactionCommand:  IRequest<GetListTransactionResponse>
{
    public List<UpdateTransactionRequest> Transactions { get; set; }
}