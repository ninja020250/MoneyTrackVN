using System.Transactions;
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
        var validator = new CreateTransactionCommandValidator(_transactionCategory);
        var createTransactionCommandResponse = new TransactionResponse();
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Count > 0)
        {
            createTransactionCommandResponse.Success = false;
            createTransactionCommandResponse.ValidationErrors = new List<string>();
            foreach (var validationError in validationResult.Errors)
                createTransactionCommandResponse.ValidationErrors.Add(validationError.ErrorMessage);

            return createTransactionCommandResponse;
        }

        var transaction = _mapper.Map<TransactionEntity>(request);
        var category = await _transactionCategory.GetByCodeAsync(request.CategoryCode);
        transaction.Category = category;

        await _transactionRepository.AddAsync(transaction);

        createTransactionCommandResponse.Transaction = _mapper.Map<GetTransactionDto>(transaction);

        return createTransactionCommandResponse;
    }
}