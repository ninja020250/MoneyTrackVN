using AutoMapper;
using MediatR;
using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Models;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Features.Users.Commands;

public class CreateUserCommandHandler(IUserRepository _userRepository, IMapper _mapper, IEmailService _emailService)
    : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
{
    public async Task<CreateUserCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<UserEntity>(request);
        var createUserCommandResponse = new CreateUserCommandResponse();
        var validator = new CreateUserCommandValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Count > 0)
        {
            createUserCommandResponse.Success = false;
            createUserCommandResponse.ValidationErrors = new List<string>();
            foreach (var validationError in validationResult.Errors)
                createUserCommandResponse.ValidationErrors.Add(validationError.ErrorMessage);
        }

        if (createUserCommandResponse.Success) user = await _userRepository.AddAsync(user);

        var email = new Email
        {
            To = "nhatcuonghuynh.tech@gmail.com",
            Body = $"A new account has been created! {user.Email}",
            Subject = "A new account has been created!"
        };

        try
        {
            await _emailService.SendEmail(email);
        }
        catch (Exception e)
        {
            // TODO: apply loggged
        }

        return createUserCommandResponse;
    }
}