using AutoMapper;
using MediatR;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Features.Transactions.Queries;
using MoneyTrack.Application.Models.Transaction;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Features.Transactions.Commands;

public class BulkUpdateTransactionCommandHandler(
    IMapper _mapper,
    ITransactionCategoryRepository _transactionCategoryRepository,
    ITransactionRepository _transactionRepository)
    : IRequestHandler<BulkUpdateTransactionCommand, GetListTransactionResponse>
{
    public async Task<GetListTransactionResponse> Handle(BulkUpdateTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var bulkValidator = new BulkUpdateTransactionCommandValidator();
        var bulkValidationResult = await bulkValidator.ValidateAsync(request, cancellationToken);

        var response = new GetListTransactionResponse();
        var updatedTransaction = new List<GetTransactionDto>();
        var validationErrors = new List<string>();

        // Check bulk validation first
        if (bulkValidationResult.Errors.Count > 0)
        {
            foreach (var error in bulkValidationResult.Errors)
            {
                validationErrors.Add(error.ErrorMessage);
            }

            response.Success = false;
            response.ValidationErrors = validationErrors;
            response.Message = "Bulk validation failed.";
            response.Transactions = new List<GetTransactionDto>();
            return response;
        }

        var individualValidator =
            new UpdateTransactionCommandValidator(_transactionRepository, _transactionCategoryRepository);

        // Validate and process each transaction
        foreach (var transactionRequest in request.Transactions)
        {
            // Create a command from the request for validation
            var command = _mapper.Map<UpdateTransactionCommand>(transactionRequest);
            var validationResult = await individualValidator.ValidateAsync(command, cancellationToken);

            if (validationResult.Errors.Count > 0)
            {
                foreach (var error in validationResult.Errors)
                {
                    validationErrors.Add(
                        $"Transaction {request.Transactions.IndexOf(transactionRequest) + 1}: {error.ErrorMessage}");
                }

                continue;
            }

            try
            {
                var transactionToUpdate = await _transactionRepository.GetByIdAsync(transactionRequest.Id);

                _mapper.Map(transactionRequest, transactionToUpdate, typeof(UpdateTransactionRequest), typeof(TransactionEntity));
                
                var category = await _transactionCategoryRepository.GetByCodeAsync(transactionRequest.CategoryCode);
                transactionToUpdate.Category = category;

                updatedTransaction.Add(_mapper.Map<GetTransactionDto>(transactionToUpdate));
        
                await _transactionRepository.UpdateAsync(transactionToUpdate);
            }
            catch (Exception ex)
            {
                validationErrors.Add(
                    $"Transaction {request.Transactions.IndexOf(transactionRequest) + 1}: Failed to create - {ex.Message}");
            }
        }

        // Set response properties
        response.Transactions = updatedTransaction;

        // Handle empty transaction list case
        if (request.Transactions.Count == 0)
        {
            response.Success = true;
            response.Message = "No transactions to update.";
        }
        else if (validationErrors.Any())
        {
            response.Success = false;
            response.ValidationErrors = validationErrors;
            response.Message =
                $"Updated {updatedTransaction.Count} out of {request.Transactions.Count} transactions. {validationErrors.Count} failed.";
        }
        else
        {
            response.Success = true;
            response.Message = $"Successfully updated {updatedTransaction.Count} transactions.";
        }

        return response;
    }
}