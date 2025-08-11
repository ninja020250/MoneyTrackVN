using MoneyTrack.Application.Models.Transaction;
using MoneyTrack.Application.Responses;

namespace MoneyTrack.Application.Features.Transactions.Queries;

public class GetListTransactionResponse : BaseResponse
{
    public List<GetTransactionDto> Transactions { get; set; }
}