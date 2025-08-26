using FluentValidation;

namespace MoneyTrack.Application.Features.AI;

public class CreateTransactionFromMessageCommandValidator : AbstractValidator<CreateTransactionFromMessageCommand>
{
    public CreateTransactionFromMessageCommandValidator()
    {
        RuleFor(x => x.Message).NotEmpty().WithMessage("Message is required");
    }
}