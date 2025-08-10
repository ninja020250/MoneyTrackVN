using FluentValidation;
using MoneyTrack.Application.Contracts.Persistence;

namespace MoneyTrack.Application.Features.Transactions.Commands;

public class UpdateTransactionCommandValidator
    : AbstractValidator<UpdateTransactionCommand>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionCategoryRepository _transactionCategoryRepository;

    public UpdateTransactionCommandValidator(
        ITransactionRepository transactionRepository,
        ITransactionCategoryRepository transactionCategoryRepository
    )
    {
        _transactionRepository = transactionRepository;
        _transactionCategoryRepository = transactionCategoryRepository;

        RuleFor(t => t).MustAsync(IsTransactionExists).WithMessage("Transaction doens't exist.");
        RuleFor(t => t)
            .MustAsync(IsCategoryCodeExist)
            .WithMessage("Category Code is not exists");
    }

    private async Task<bool> IsCategoryCodeExist(UpdateTransactionCommand e, CancellationToken token)
    {
        return await _transactionCategoryRepository.IsCategoryCodeExist(e.CategoryCode);
    }

    private async Task<bool> IsTransactionExists(UpdateTransactionCommand e, CancellationToken cancellationToken)
    {
        return await _transactionRepository.IsTransactionExist(e.Id);
    }
}