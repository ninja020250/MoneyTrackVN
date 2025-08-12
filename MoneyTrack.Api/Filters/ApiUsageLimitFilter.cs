using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Exceptions;
using MoneyTrack.Application.Features.ApiUsage;

namespace MoneyTrack.Api.Filters;

public class ApiUsageLimitFilter : IAsyncActionFilter
{
    private readonly IMediator _mediator;
    private readonly IDictionary<string, int> _apiLimits;
    private readonly ICurrentUserService _currentUserService;
    private readonly IApiUsageRepository _apiUsageRepository;

    public ApiUsageLimitFilter(
        IApiUsageRepository apiUsageRepository,
        ICurrentUserService currentUserService,
        IOptions<Dictionary<string, int>> apiLimits,
        IMediator mediator
    )
    {
        _apiUsageRepository = apiUsageRepository;
        _currentUserService = currentUserService;
        _apiLimits = apiLimits.Value;
        _mediator = mediator;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userId = _currentUserService.UserId;
        if (userId is null)
        {
            await next();
            return;
        }

        var apiName = context.ActionDescriptor.DisplayName;

        if (_apiLimits.TryGetValue(apiName, out var limit))
        {
            var usage = await _apiUsageRepository.getTodayUsageByName(userId.Value, apiName);

            if (usage != null && usage.CallCount >= limit)
            {
                throw new BadRequestException("You have reached the daily limit for this API.");
            }

            var result = await next();

            if (result.Exception == null || result.ExceptionHandled)
            {
                var command = new UpsertApiUsageCommand() { ApiName = apiName, UserId = userId.Value };
                _mediator.Send(command);
            }
        }
        else
        {
            await next();
        }
    }
}