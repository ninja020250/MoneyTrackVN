using AutoMapper;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Features.Transactions.Commands;
using MoneyTrack.Application.Features.Transactions.Queries;
using MoneyTrack.Application.Models.Transaction;
using MoneyTrack.Application.Profiles;
using MoneyTrack.Application.UnitTests.Mocks;
using Moq;
using Shouldly;

namespace MoneyTrack.Application.UnitTests.Transactions.Commands;

public class BulkUpdateTransactionCommandHandlerTest
{
    private readonly IMapper _mapper;
    private readonly Mock<ITransactionRepository> _mockTransactionRepository;
    private readonly Mock<ITransactionCategoryRepository> _mockTransactionCategoryRepository;

    public BulkUpdateTransactionCommandHandlerTest()
    {
        _mockTransactionRepository = RepositoryMocks.GetTransactionRepository();
        _mockTransactionCategoryRepository = RepositoryMocks.GetTransactionCategoryRepository();
        var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });
        _mapper = configurationProvider.CreateMapper();
    }

    [Fact]
    public async Task BulkUpdateTransactionTest_Success()
    {
        var handler = new BulkUpdateTransactionCommandHandler(
            _mapper,
            _mockTransactionCategoryRepository.Object,
            _mockTransactionRepository.Object
        );

        var command = new BulkUpdateTransactionCommand
        {
            Transactions = new List<UpdateTransactionRequest>
            {
                new UpdateTransactionRequest
                {
                    Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                    Description = "Updated Transaction 1",
                    Amount = 120.50,
                    ExpenseDate = DateTime.Now,
                    CategoryCode = "FOOD_001"
                },
                new UpdateTransactionRequest
                {
                    Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa7"),
                    Description = "Updated Transaction 2",
                    Amount = 85.25,
                    ExpenseDate = DateTime.Now.AddDays(-1),
                    CategoryCode = "SHOPPING_001"
                }
            }
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldBeOfType<GetListTransactionResponse>();
        result.Success.ShouldBeTrue();
        result.Transactions.ShouldNotBeNull();
        result.Transactions.Count.ShouldBe(2);
        result.Transactions[0].Description.ShouldBe("Updated Transaction 1");
        result.Transactions[1].Description.ShouldBe("Updated Transaction 2");
    }

    [Fact]
    public async Task BulkUpdateTransactionTest_EmptyList_ShouldReturnEmptyResult()
    {
        var handler = new BulkUpdateTransactionCommandHandler(
            _mapper,
            _mockTransactionCategoryRepository.Object,
            _mockTransactionRepository.Object);

        var command = new BulkUpdateTransactionCommand
        {
            Transactions = new List<UpdateTransactionRequest>()
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldBeOfType<GetListTransactionResponse>();
        result.Success.ShouldBeTrue();
        result.Transactions.ShouldNotBeNull();
        result.Transactions.Count.ShouldBe(0);
    }

    [Fact]
    public async Task BulkUpdateTransactionTest_WithNonExistentTransaction_ShouldFail()
    {
        var handler = new BulkUpdateTransactionCommandHandler(
            _mapper,
            _mockTransactionCategoryRepository.Object,
            _mockTransactionRepository.Object);

        var command = new BulkUpdateTransactionCommand
        {
            Transactions = new List<UpdateTransactionRequest>
            {
                new UpdateTransactionRequest
                {
                    Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                    Description = "Valid Update",
                    Amount = 120.50,
                    ExpenseDate = DateTime.Now,
                    CategoryCode = "FOOD_001"
                },
                new UpdateTransactionRequest
                {
                    Id = Guid.NewGuid(), // Non-existent ID
                    Description = "Invalid Update",
                    Amount = 85.25,
                    ExpenseDate = DateTime.Now,
                    CategoryCode = "SHOPPING_001"
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
    public async Task BulkUpdateTransactionTest_WithInvalidCategory_ShouldFail()
    {
        var handler = new BulkUpdateTransactionCommandHandler(
            _mapper,
            _mockTransactionCategoryRepository.Object,
            _mockTransactionRepository.Object);

        var command = new BulkUpdateTransactionCommand
        {
            Transactions = new List<UpdateTransactionRequest>
            {
                new UpdateTransactionRequest
                {
                    Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                    Description = "Valid Update",
                    Amount = 120.50,
                    ExpenseDate = DateTime.Now,
                    CategoryCode = "FOOD_001"
                },
                new UpdateTransactionRequest
                {
                    Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa7"),
                    Description = "Invalid Category Update",
                    Amount = 85.25,
                    ExpenseDate = DateTime.Now,
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
    public async Task BulkUpdateTransactionTest_LargeList_ShouldSucceed()
    {
        var handler = new BulkUpdateTransactionCommandHandler(
            _mapper,
            _mockTransactionCategoryRepository.Object,
            _mockTransactionRepository.Object
        );

        var transactions = new List<UpdateTransactionRequest>();
        var baseId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");

        for (int i = 0; i < 5; i++)
        {
            transactions.Add(new UpdateTransactionRequest()
            {
                Id = baseId, // Using same ID for simplicity in mock
                Description = $"Bulk Update Transaction {i + 1}",
                Amount = (i + 1) * 15.0,
                ExpenseDate = DateTime.Now.AddDays(-i),
                CategoryCode = "FOOD_001"
            });
        }

        var command = new BulkUpdateTransactionCommand
        {
            Transactions = transactions
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldBeOfType<GetListTransactionResponse>();
        result.Success.ShouldBeTrue();
        result.Transactions.Count.ShouldBe(5);
    }
}