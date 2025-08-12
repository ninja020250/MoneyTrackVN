using MediatR;
using MoneyTrack.Application.Responses;

namespace MoneyTrack.Application.Features.Transactions.Commands;

public class BulkDeleteTransactionCommand : IRequest<BaseResponse>
{
    public List<Guid> TransactionIds { get; set; } = new();
}