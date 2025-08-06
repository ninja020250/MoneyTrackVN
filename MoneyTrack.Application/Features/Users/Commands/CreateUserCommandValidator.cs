using FluentValidation;

namespace MoneyTrack.Application.Features.Users.Commands;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(p => p.Email).NotEmpty().WithMessage("{PropertyName} is required.}");
        RuleFor(p => p.Username).NotEmpty().WithMessage("{PropertyName} is required.}");
        RuleFor(p => p.PasswordHash).NotEmpty().WithMessage("{PropertyName} is required.}");
    }
}