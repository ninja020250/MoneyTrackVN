using AutoMapper;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Features.Transactions.Commands;
using MoneyTrack.Application.Features.Transactions.Commands.CreateTransaction;
using MoneyTrack.Application.Features.Transactions.Queries;
using MoneyTrack.Application.Models.Transaction;
using MoneyTrack.Application.Profiles;
using MoneyTrack.Application.UnitTests.Mocks;
using Moq;
using Shouldly;

namespace MoneyTrack.Application.UnitTests.Transactions.Commands;

public class BulkCreateTransactionCommandHandlerTest
{
    private readonly IMapper _mapper;
    private readonly Mock<ITransactionRepository> _mockTransactionRepository;
    private readonly Mock<ITransactionCategoryRepository> _mockTransactionCategoryRepository;

    public BulkCreateTransactionCommandHandlerTest()
    {
        _mockTransactionRepository = RepositoryMocks.GetTransactionRepository();
        _mockTransactionCategoryRepository = RepositoryMocks.GetTransactionCategoryRepository();
        var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });
        _mapper = configurationProvider.CreateMapper();
    }

    [Fact]
    public async Task BulkCreateTransactionTest_Success()
    {
        var handler = new BulkCreateTransactionCommandHandler(
            _mockTransactionRepository.Object,
            _mockTransactionCategoryRepository.Object,
            _mapper);

        var command = new BulkCreateTransactionCommand
        {
            Transactions = new List<CreateTransactionRequest>
            {
                new CreateTransactionRequest
                {
                    Description = "Test Transaction 1",
                    Amount = 100.50,
                    ExpenseDate = DateTime.Now,
                    UserId = Guid.NewGuid(),
                    CategoryCode = "FOOD_001"
                },
                new CreateTransactionRequest
                {
                    Description = "Test Transaction 2",
                    Amount = 75.25,
                    ExpenseDate = DateTime.Now.AddDays(-1),
                    UserId = Guid.NewGuid(),
                    CategoryCode = "SHOPPING_001"
                }
            }
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldBeOfType<GetListTransactionResponse>();
        result.Success.ShouldBeTrue();
        result.Transactions.ShouldNotBeNull();
        result.Transactions.Count.ShouldBe(2);
        result.Transactions[0].Description.ShouldBe("Test Transaction 1");
        result.Transactions[1].Description.ShouldBe("Test Transaction 2");
    }

    [Fact]
    public async Task BulkCreateTransactionTest_EmptyList_ShouldReturnEmptyResult()
    {
        var handler = new BulkCreateTransactionCommandHandler(
            _mockTransactionRepository.Object,
            _mockTransactionCategoryRepository.Object,
            _mapper);

        var command = new BulkCreateTransactionCommand
        {
            Transactions = new List<CreateTransactionRequest>()
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldBeOfType<GetListTransactionResponse>();
        result.Success.ShouldBeTrue();
        result.Transactions.ShouldNotBeNull();
        result.Transactions.Count.ShouldBe(0);
    }

    [Fact]
    public async Task BulkCreateTransactionTest_WithInvalidCategory_ShouldFail()
    {
        var handler = new BulkCreateTransactionCommandHandler(
            _mockTransactionRepository.Object,
            _mockTransactionCategoryRepository.Object,
            _mapper);

        var command = new BulkCreateTransactionCommand
        {
            Transactions = new List<CreateTransactionRequest>
            {
                new CreateTransactionRequest
                {
                    Description = "Valid Transaction",
                    Amount = 100.50,
                    ExpenseDate = DateTime.Now,
                    UserId = Guid.NewGuid(),
                    CategoryCode = "FOOD_001"
                },
                new CreateTransactionRequest
                {
                    Description = "Invalid Transaction",
                    Amount = 75.25,
                    ExpenseDate = DateTime.Now,
                    UserId = Guid.NewGuid(),
                    CategoryCode = "INVALID_CODE"
                }
            }
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldBeOfType<GetListTransactionResponse>();
        result.Success.ShouldBeFalse();
        result.ValidationErrors.ShouldNotBeNull();
        result.ValidationErrors.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task BulkCreateTransactionTest_LargeList_ShouldSucceed()
    {
        var handler = new BulkCreateTransactionCommandHandler(
            _mockTransactionRepository.Object,
            _mockTransactionCategoryRepository.Object,
            _mapper);

        var transactions = new List<CreateTransactionRequest>();
        for (int i = 1; i <= 10; i++)
        {
            transactions.Add(new CreateTransactionRequest
            {
                Description = $"Bulk Transaction {i}",
                Amount = i * 10.0,
                ExpenseDate = DateTime.Now.AddDays(-i),
                UserId = Guid.NewGuid(),
                CategoryCode = "FOOD_001"
            });
        }

        var command = new BulkCreateTransactionCommand
        {
            Transactions = transactions
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldBeOfType<GetListTransactionResponse>();
        result.Success.ShouldBeTrue();
        result.Transactions.Count.ShouldBe(10);
    }
}