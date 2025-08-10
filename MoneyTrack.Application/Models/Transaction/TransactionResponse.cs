using MoneyTrack.Application.Responses;

namespace MoneyTrack.Application.Models.Transaction;

public class TransactionResponse: BaseResponse
{
    public GetTransactionDto? Transaction { get; set; }
}