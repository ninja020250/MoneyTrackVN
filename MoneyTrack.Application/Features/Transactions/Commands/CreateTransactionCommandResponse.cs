using MoneyTrack.Application.Models.Transaction;
using MoneyTrack.Application.Responses;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Features.Transactions.Commands;

public class CreateTransactionCommandResponse : BaseResponse
{
   public GetTransactionDto? Transaction { get; set; }
}