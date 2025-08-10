using MediatR;
using MoneyTrack.Application.Responses;

namespace MoneyTrack.Application.Features.Transactions.Commands;

public class DeleteTransactionCommand : IRequest<BaseResponse>
{
    public Guid Id { get; private set; }
}