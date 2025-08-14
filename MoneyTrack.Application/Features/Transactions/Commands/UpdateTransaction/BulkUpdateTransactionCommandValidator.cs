using FluentValidation;

namespace MoneyTrack.Application.Features.Transactions.Commands;

public class BulkUpdateTransactionCommandValidator: AbstractValidator<BulkUpdateTransactionCommand>
{
    public BulkUpdateTransactionCommandValidator()
    {
        RuleFor(x => x.Transactions)
            .NotNull()
            .WithMessage("Transactions list cannot be null.");

        // Allow empty lists for bulk updates - this is a valid scenario
        // Individual transaction validation will be handled separately
    }
}