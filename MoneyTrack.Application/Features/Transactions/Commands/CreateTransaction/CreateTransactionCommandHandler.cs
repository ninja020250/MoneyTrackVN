using AutoMapper;
using MediatR;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Models.Transaction;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Features.Transactions.Commands;

public class CreateTransactionCommandHandler(
    ITransactionRepository _transactionRepository,
    ITransactionCategoryRepository _transactionCategory,
    IMapper _mapper
)
    : IRequestHandler<CreateTransactionCommand, TransactionResponse>
{
    public async Task<TransactionResponse> Handle(CreateTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var createTransactionCommandResponse = new TransactionResponse();

        var transaction = _mapper.Map<TransactionEntity>(request);
        var category = await _transactionCategory.GetByCodeAsync(request.CategoryCode);
        transaction.Category = category;

        await _transactionRepository.AddAsync(transaction);

        createTransactionCommandResponse.Transaction = _mapper.Map<GetTransactionDto>(transaction);

        return createTransactionCommandResponse;
    }
}