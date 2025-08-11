using MoneyTrack.Application.Models.Transaction;
using MoneyTrack.Application.Responses;

namespace MoneyTrack.Application.Features.AI;

public class CreateTransactionFromMessageCommandResponse: BaseResponse
{
    public GetTransactionDto? transaction { get; set; }
}