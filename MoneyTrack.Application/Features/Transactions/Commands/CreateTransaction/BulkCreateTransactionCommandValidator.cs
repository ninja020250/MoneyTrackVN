using FluentValidation;
using MoneyTrack.Application.Contracts.Persistence;

namespace MoneyTrack.Application.Features.Transactions.Commands;

public class BulkCreateTransactionCommandValidator
    : AbstractValidator<BulkCreateTransactionCommand>
{
    public BulkCreateTransactionCommandValidator()
    {
        RuleFor(x => x.Transactions)
            .NotNull()
            .WithMessage("Transactions list cannot be null.");
    }
}