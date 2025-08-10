using AutoMapper;
using MediatR;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Features.Transactions.Queries;
using MoneyTrack.Application.Models.Transaction;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Features.Transactions.Commands;

public class BulkCreateTransactionCommandHandler(
    ITransactionRepository _transactionRepository,
    ITransactionCategoryRepository _transactionCategory,
    IMapper _mapper)
    : IRequestHandler<BulkCreateTransactionCommand, GetTransactionResponse>
{
    public async Task<GetTransactionResponse> Handle(BulkCreateTransactionCommand request,
        CancellationToken cancellationToken)
    {
        // Validate the bulk command itself
        var bulkValidator = new BulkCreateTransactionCommandValidator();
        var bulkValidationResult = await bulkValidator.ValidateAsync(request, cancellationToken);
        
        var response = new GetTransactionResponse();
        var createdTransactions = new List<GetTransactionDto>();
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
        
        var individualValidator = new CreateTransactionCommandValidator(_transactionCategory);

        // Validate and process each transaction
        foreach (var transactionRequest in request.Transactions)
        {
            // Create a command from the request for validation
            var command = _mapper.Map<CreateTransactionCommand>(transactionRequest);
            var validationResult = await individualValidator.ValidateAsync(command, cancellationToken);

            if (validationResult.Errors.Count > 0)
            {
                foreach (var error in validationResult.Errors)
                {
                    validationErrors.Add($"Transaction {request.Transactions.IndexOf(transactionRequest) + 1}: {error.ErrorMessage}");
                }
                continue;
            }

            try
            {
                // Map to entity and set category
                var transaction = _mapper.Map<TransactionEntity>(transactionRequest);
                var category = await _transactionCategory.GetByCodeAsync(transactionRequest.CategoryCode);
                transaction.Category = category;
                transaction.CategoryId = category.Id;

                // Save transaction
                await _transactionRepository.AddAsync(transaction);

                // Map to DTO and add to results
                var transactionDto = _mapper.Map<GetTransactionDto>(transaction);
                createdTransactions.Add(transactionDto);
            }
            catch (Exception ex)
            {
                validationErrors.Add($"Transaction {request.Transactions.IndexOf(transactionRequest) + 1}: Failed to create - {ex.Message}");
            }
        }

        // Set response properties
        response.Transactions = createdTransactions;
        
        if (validationErrors.Any())
        {
            response.Success = false;
            response.ValidationErrors = validationErrors;
            response.Message = $"Created {createdTransactions.Count} out of {request.Transactions.Count} transactions. {validationErrors.Count} failed.";
        }
        else
        {
            response.Success = true;
            response.Message = $"Successfully created {createdTransactions.Count} transactions.";
        }

        return response;
    }
}