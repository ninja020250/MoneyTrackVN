using MediatR;

namespace MoneyTrack.Application.Features.ApiUsage;

public class UpsertApiUsageCommand : IRequest
{
    public string ApiName;
    
    public Guid UserId;
}