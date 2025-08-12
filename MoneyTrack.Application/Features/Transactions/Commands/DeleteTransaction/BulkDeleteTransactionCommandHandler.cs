using MediatR;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Responses;

namespace MoneyTrack.Application.Features.Transactions.Commands;

public class BulkDeleteTransactionCommandHandler(
    ITransactionRepository _transactionRepository
) : IRequestHandler<BulkDeleteTransactionCommand, BaseResponse>
{
    public async Task<BaseResponse> Handle(BulkDeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse();
        var deletedCount = 0;
        var notFoundIds = new List<Guid>();
        var errors = new List<string>();

        foreach (var transactionId in request.TransactionIds)
        {
            try
            {
                var transaction = await _transactionRepository.GetByIdAsync(transactionId);
                if (transaction == null)
                {
                    notFoundIds.Add(transactionId);
                    continue;
                }

                await _transactionRepository.DeleteAsync(transaction);
                deletedCount++;
            }
            catch (Exception ex)
            {
                errors.Add($"Failed to delete transaction {transactionId}: {ex.Message}");
            }
        }

        if (errors.Count > 0 || notFoundIds.Count > 0)
        {
            response.Success = deletedCount > 0;
            var errorMessages = new List<string>();

            if (notFoundIds.Count > 0)
            {
                errorMessages.Add($"Transactions not found: {string.Join(", ", notFoundIds)}");
            }

            if (errors.Count > 0)
            {
                errorMessages.AddRange(errors);
            }

            response.Message = $"Deleted {deletedCount} out of {request.TransactionIds.Count} transactions. Errors: {string.Join("; ", errorMessages)}";
        }
        else
        {
            response.Success = true;
            response.Message = $"Successfully deleted {deletedCount} transactions.";
        }

        return response;
    }
}