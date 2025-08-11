using MediatR;

namespace MoneyTrack.Application.Features.Transactions.Queries;

public class GetTransactionsQuery : IRequest<GetListTransactionResponse>
{
    public string UserId { get; init; }
}