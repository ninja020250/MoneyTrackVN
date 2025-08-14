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
        // Check for cancellation at the start
        cancellationToken.ThrowIfCancellationRequested();
        
        var response = new BaseResponse();
        var deletedCount = 0;
        var notFoundIds = new List<Guid>();
        var errors = new List<string>();

        // Handle empty list case
        if (request.TransactionIds == null || !request.TransactionIds.Any())
        {
            response.Success = true;
            response.Message = "No transactions to delete.";
            return response;
        }

        foreach (var transactionId in request.TransactionIds)
        {
            // Check for cancellation during processing
            cancellationToken.ThrowIfCancellationRequested();
            
            try
            {
                // Check for empty GUID
                if (transactionId == Guid.Empty)
                {
                    throw new Exception($"Invalid transaction ID: {transactionId}");
                }
                
                var transaction = await _transactionRepository.GetByIdAsync(transactionId);
                if (transaction == null)
                {
                    throw new Exception($"Transaction with ID {transactionId} not found.");
                }

                await _transactionRepository.DeleteAsync(transaction);
                deletedCount++;
            }
            catch (OperationCanceledException)
            {
                throw; // Re-throw cancellation exceptions
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete transaction {transactionId}: {ex.Message}", ex);
            }
        }

        response.Success = true;
        response.Message = $"Successfully deleted {deletedCount} transactions.";
        return response;
    }
}