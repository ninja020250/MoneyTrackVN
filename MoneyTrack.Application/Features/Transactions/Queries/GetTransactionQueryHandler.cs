using AutoMapper;
using MediatR;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Exceptions;
using MoneyTrack.Application.Models.Transaction;

namespace MoneyTrack.Application.Features.Transactions.Queries;

public class GetTransactionQueryHandler(
    ITransactionRepository _transactionRepository,
    IMapper _mapper
)
    : IRequestHandler<GetTransactionsQuery, GetListTransactionResponse>
{
    public async Task<GetListTransactionResponse> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _transactionRepository.GetListByUserIdAsync(new Guid(request.UserId));

        var response = new GetListTransactionResponse();

        response.Transactions = _mapper.Map<List<GetTransactionDto>>(transactions);

        return response;
    }
}