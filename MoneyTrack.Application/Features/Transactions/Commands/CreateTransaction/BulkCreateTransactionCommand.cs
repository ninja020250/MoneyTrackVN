using MediatR;
using MoneyTrack.Application.Features.Transactions.Queries;
using MoneyTrack.Application.Models.Transaction;

namespace MoneyTrack.Application.Features.Transactions.Commands;

public class BulkCreateTransactionCommand : IRequest<GetListTransactionResponse>
{
    public List<CreateTransactionRequest> Transactions { get; set; }
}