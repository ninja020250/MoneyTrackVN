using MediatR;
using MoneyTrack.Application.Features.Transactions.Queries;
using MoneyTrack.Application.Models.AI;

namespace MoneyTrack.Application.Features.AI;

public class CreateTransactionFromMessageCommand : CreateTransactionFromMessageRequest, IRequest<CreateTransactionFromMessageCommandResponse>
{
    public Guid UserId { get; set; }
}