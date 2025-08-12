using FluentValidation;

namespace MoneyTrack.Application.Features.Transactions.Commands;

public class BulkDeleteTransactionCommandValidator : AbstractValidator<BulkDeleteTransactionCommand>
{
    public BulkDeleteTransactionCommandValidator()
    {
        RuleFor(x => x.TransactionIds)
            .NotNull()
            .WithMessage("Transaction IDs list cannot be null.");

        RuleFor(x => x.TransactionIds)
            .NotEmpty()
            .WithMessage("At least one transaction ID is required.");

        RuleFor(x => x.TransactionIds)
            .Must(ids => ids.All(id => id != Guid.Empty))
            .WithMessage("All transaction IDs must be valid (non-empty GUIDs).");

        RuleFor(x => x.TransactionIds)
            .Must(ids => ids.Distinct().Count() == ids.Count)
            .WithMessage("Duplicate transaction IDs are not allowed.");
    }
}