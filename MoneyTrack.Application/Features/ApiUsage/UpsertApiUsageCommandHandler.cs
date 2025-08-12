using MediatR;
using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Features.ApiUsage;

public class UpsertApiUsageCommandHandler(
    IApiUsageRepository _iApiUsageRepository
) : IRequestHandler<UpsertApiUsageCommand>
{
    public async Task Handle(UpsertApiUsageCommand request, CancellationToken cancellationToken)
    {
        var usage = await _iApiUsageRepository.getTodayUsageByName(request.UserId, request.ApiName);

        if (usage == null)
        {
            usage = new ApiUsageEntity()
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                ApiName = request.ApiName,
                CallDate = DateTime.UtcNow.Date,
                CallCount = 1
            };
            _iApiUsageRepository.AddAsync(usage);
        }
        else
        {
            usage.CallCount++;
            _iApiUsageRepository.UpdateAsync(usage);
        }
    }
}