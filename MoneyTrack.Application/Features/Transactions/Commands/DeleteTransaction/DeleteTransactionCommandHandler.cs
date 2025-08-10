using MediatR;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Exceptions;
using MoneyTrack.Application.Responses;

namespace MoneyTrack.Application.Features.Transactions.Commands;

public class DeleteTransactionCommandHandler(ITransactionRepository _transactionRepository)
    : IRequestHandler<DeleteTransactionCommand, BaseResponse>
{
    public async Task<BaseResponse> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.Id);
        if (transaction == null)
        {
            throw new NotFoundException("Transaction", "id");
        }

        await _transactionRepository.DeleteAsync(transaction);
        return new BaseResponse();
    }
}