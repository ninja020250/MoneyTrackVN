using MediatR;
using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Application.Exceptions;
using Microsoft.Extensions.Options;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Responses;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Features.ApiUsage;

public class ApiUsageLimitBehavior<TRequest, TResponse>(
    IApiUsageRepository apiUsageRepository,
    ICurrentUserService currentUserService,
    IOptions<Dictionary<string, int>> apiLimits)
    : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IDictionary<string, int> _apiLimits = apiLimits.Value;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId is null)
        {
            return await next();
        }

        var apiName = typeof(TRequest).Name;
        if (_apiLimits.TryGetValue(apiName, out var limit))
        {
            var usage = await apiUsageRepository.getTodayUsageByName(userId.Value, apiName);
            if (usage != null && usage.CallCount >= limit)
            {
                throw new BadRequestException("API_LIMIT_EXCEEDED");
            }

            var response = await next();
            // Upsert logic (mirroring UpsertApiUsageCommandHandler)
            if (usage == null)
            {
                usage = new ApiUsageEntity()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId.Value,
                    ApiName = apiName,
                    CallDate = DateTime.UtcNow.Date,
                    CallCount = 1
                };
                await apiUsageRepository.AddAsync(usage);
            }
            else
            {
                usage.CallCount++;
                await apiUsageRepository.UpdateAsync(usage);
            }

            return response;
        }
        else
        {
            return await next();
        }
    }
}