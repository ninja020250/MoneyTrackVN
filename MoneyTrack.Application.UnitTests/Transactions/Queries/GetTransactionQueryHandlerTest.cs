using AutoMapper;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Features.Transactions.Queries;
using MoneyTrack.Application.Models.Transaction;
using MoneyTrack.Application.Profiles;
using MoneyTrack.Application.UnitTests.Mocks;
using MoneyTrack.Domain.Entities;
using Moq;
using Shouldly;

namespace MoneyTrack.Application.UnitTests.Transactions.Queries;

public class GetTransactionQueryHandlerTest
{
    private readonly IMapper _mapper;
    private readonly Mock<ITransactionRepository> _mockTransactionRepository;

    public GetTransactionQueryHandlerTest()
    {
        _mockTransactionRepository = RepositoryMocks.GetTransactionRepository();
        var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });
        _mapper = configurationProvider.CreateMapper();
    }

    [Fact]
    public async Task GetListTransactionByUserIdTest()
    {
        var handler = new GetTransactionQueryHandler(_mockTransactionRepository.Object, _mapper);

        var result =
            await handler.Handle(
                new GetTransactionsQuery() { UserId = "b34ac404-2285-4af1-88f6-1e4692d6df45" },
                CancellationToken.None);

        result.ShouldBeOfType<GetListTransactionResponse>();
        result.Transactions.ShouldBeOfType<List<GetTransactionDto>>();
        result.Transactions.Count.ShouldBe(6);
    }
}