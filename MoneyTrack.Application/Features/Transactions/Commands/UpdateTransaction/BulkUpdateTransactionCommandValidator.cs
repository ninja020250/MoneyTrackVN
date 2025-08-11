using FluentValidation;

namespace MoneyTrack.Application.Features.Transactions.Commands;

public class BulkUpdateTransactionCommandValidator: AbstractValidator<BulkUpdateTransactionCommand>
{
    public BulkUpdateTransactionCommandValidator()
    {
        RuleFor(x => x.Transactions)
            .NotNull()
            .WithMessage("Transactions list cannot be null.");

        RuleFor(x => x.Transactions)
            .NotEmpty()
            .WithMessage("At least one transaction is required.");
    }
}