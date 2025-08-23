using FluentValidation;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Features.Users.Commands;

namespace MoneyTrack.Application.Features.Transactions.Commands;

public class CreateTransactionCommandValidator
    : AbstractValidator<CreateTransactionCommand>
{
    private ITransactionCategoryRepository _transactionCategory;

    public CreateTransactionCommandValidator(ITransactionCategoryRepository transactionCategory)
    {
        _transactionCategory = transactionCategory;

        RuleFor(t => t.Amount).NotEmpty().GreaterThan(0);
        RuleFor(t => t.UserId).NotEmpty();
        RuleFor(t => t.ExpenseDate).NotEmpty();
        RuleFor(t => t.Description).NotEmpty();

        RuleFor(t => t.CategoryCode).NotEmpty();
        RuleFor(t => t)
            .MustAsync(IsCategoryCodeExist)
            .WithMessage("Category Code is not exists");
    }

    private async Task<bool> IsCategoryCodeExist(CreateTransactionCommand e, CancellationToken token)
    {
        return await _transactionCategory.IsCategoryCodeExist(e.CategoryCode);
    }
}