using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Exceptions;
using MoneyTrack.Application.Features.ApiUsage;

namespace MoneyTrack.Api.Filters;

public class ApiUsageLimitFilter(
    IApiUsageRepository apiUsageRepository,
    ICurrentUserService currentUserService,
    IOptions<Dictionary<string, int>> apiLimits,
    IMediator mediator)
    : IAsyncActionFilter
{
    private readonly IDictionary<string, int> _apiLimits = apiLimits.Value;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userId = currentUserService.UserId;
        if (userId is null)
        {
            await next();
            return;
        }

        var apiName = context.ActionDescriptor.DisplayName;

        if (_apiLimits.TryGetValue(apiName, out var limit))
        {
            var usage = await apiUsageRepository.getTodayUsageByName(userId.Value, apiName);

            if (usage != null && usage.CallCount >= limit)
            {
                throw new BadRequestException("API_LIMIT_EXCEEDED");
            }

            var result = await next();

            if (result.Exception == null || result.ExceptionHandled)
            {
                var command = new UpsertApiUsageCommand() { ApiName = apiName, UserId = userId.Value };
                mediator.Send(command);
            }
        }
        else
        {
            await next();
        }
    }
}