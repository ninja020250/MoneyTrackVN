using System.Transactions;
using AutoMapper;
using MediatR;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Models.Transaction;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Features.Transactions.Commands;

public class UpdateTransactionCommandHandler(
    IMapper _mapper,
    ITransactionRepository _transactionRepository,
    ITransactionCategoryRepository _transactionCategoryRepository
) : IRequestHandler<UpdateTransactionCommand, TransactionResponse>
{
    public async Task<TransactionResponse> Handle(UpdateTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var validator = new UpdateTransactionCommandValidator(_transactionRepository, _transactionCategoryRepository);
        var updateTransactionCommandResponse = new TransactionResponse();
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Count > 0)
        {
            updateTransactionCommandResponse.Success = false;
            updateTransactionCommandResponse.ValidationErrors = new List<string>();
            foreach (var validationError in validationResult.Errors)
                updateTransactionCommandResponse.ValidationErrors.Add(validationError.ErrorMessage);

            return updateTransactionCommandResponse;
        }

        var transactionToUpdate = await _transactionRepository.GetByIdAsync(request.Id);

        _mapper.Map(request, transactionToUpdate, typeof(UpdateTransactionCommand), typeof(TransactionEntity));
     
        var category = await _transactionCategoryRepository.GetByCodeAsync(request.CategoryCode);
        transactionToUpdate.Category = category;

        updateTransactionCommandResponse.Transaction = _mapper.Map<GetTransactionDto>(transactionToUpdate);
        
        await _transactionRepository.UpdateAsync(transactionToUpdate);

        return updateTransactionCommandResponse;
    }
}