using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MoneyTrack.Application.Common.Behaviours;
using MoneyTrack.Application.Features.AI;
using MoneyTrack.Application.Features.Transactions.Commands;
using MoneyTrack.Application.Features.Users.Commands;

namespace MoneyTrack.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Features.ApiUsage.ApiUsageLimitBehavior<,>));
        services.AddValidatorService();
        
        return services;
    }

    private static IServiceCollection AddValidatorService(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssemblyContaining<CreateTransactionFromMessageCommand>();
        services.AddValidatorsFromAssemblyContaining<CreateTransactionCommand>();
        services.AddValidatorsFromAssemblyContaining<UpdateTransactionCommand>();
        services.AddValidatorsFromAssemblyContaining<CreateUserCommand>();
        
        return services;
    }
}