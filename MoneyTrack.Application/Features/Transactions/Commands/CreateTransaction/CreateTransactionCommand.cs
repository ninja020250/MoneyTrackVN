using System.Transactions;
using MediatR;
using MoneyTrack.Application.Models.Transaction;

namespace MoneyTrack.Application.Features.Transactions.Commands;

public class CreateTransactionCommand : CreateTransactionRequest, IRequest<TransactionResponse>
{
}