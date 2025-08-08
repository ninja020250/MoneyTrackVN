using MediatR;

namespace MoneyTrack.Application.Features.Transactions.Queries;

public class GetTransactionsQuery : IRequest<GetTransactionResponse>
{
    public string UserId { get; init; }
}