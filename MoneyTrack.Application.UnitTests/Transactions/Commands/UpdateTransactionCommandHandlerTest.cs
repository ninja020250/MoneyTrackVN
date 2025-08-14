using AutoMapper;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Features.Transactions.Commands;
using MoneyTrack.Application.Models.Transaction;
using MoneyTrack.Application.Profiles;
using MoneyTrack.Application.UnitTests.Mocks;
using MoneyTrack.Domain.Entities;
using Moq;
using Shouldly;

namespace MoneyTrack.Application.UnitTests.Transactions.Commands;

public class UpdateTransactionCommandHandlerTest
{
    private readonly IMapper _mapper;
    private readonly Mock<ITransactionRepository> _mockTransactionRepository;
    private readonly Mock<ITransactionCategoryRepository> _mockTransactionCategoryRepository;

    public UpdateTransactionCommandHandlerTest()
    {
        _mockTransactionRepository = RepositoryMocks.GetTransactionRepository();
        _mockTransactionCategoryRepository = RepositoryMocks.GetTransactionCategoryRepository();
        var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });
        _mapper = configurationProvider.CreateMapper();
    }

    [Fact]
    public async Task UpdateTransactionTest_Success()
    {
        var handler = new UpdateTransactionCommandHandler(
            _mapper,
            _mockTransactionRepository.Object,
            _mockTransactionCategoryRepository.Object
        );

        var existingTransactionId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
        var command = new UpdateTransactionCommand
        {
            Id = existingTransactionId,
            Description = "Updated Transaction",
            Amount = 150.75,
            ExpenseDate = DateTime.Now.AddDays(-1),
            CategoryCode = "SHOPPING_001"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldBeOfType<TransactionResponse>();
        result.Success.ShouldBeTrue();
        result.Transaction.ShouldNotBeNull();
        result.Transaction.Id.ShouldBe(command.Id);
        result.Transaction.Description.ShouldBe(command.Description);
        result.Transaction.Amount.ShouldBe(command.Amount);
        result.Transaction.Category.Code.ShouldBe(command.CategoryCode);
    }

    [Fact]
    public async Task UpdateTransactionTest_NonExistentTransaction_ShouldFail()
    {
        var handler = new UpdateTransactionCommandHandler(
            _mapper,
            _mockTransactionRepository.Object,
            _mockTransactionCategoryRepository.Object
        );

        var nonExistentId = Guid.NewGuid();
        var command = new UpdateTransactionCommand
        {
            Id = nonExistentId,
            Description = "Updated Transaction",
            Amount = 150.75,
            ExpenseDate = DateTime.Now,
            CategoryCode = "FOOD_001"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldBeOfType<TransactionResponse>();
        result.Success.ShouldBeFalse();
        result.ValidationErrors.ShouldNotBeNull();
        result.ValidationErrors.ShouldContain("Transaction doens't exist.");
    }

    [Fact]
    public async Task UpdateTransactionTest_InvalidCategory_ShouldFail()
    {
        var handler = new UpdateTransactionCommandHandler(
            _mapper,
            _mockTransactionRepository.Object,
            _mockTransactionCategoryRepository.Object);

        var existingTransactionId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
        var command = new UpdateTransactionCommand
        {
            Id = existingTransactionId,
            Description = "Updated Transaction",
            Amount = 150.75,
            ExpenseDate = DateTime.Now,
            CategoryCode = "INVALID_CODE"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldBeOfType<TransactionResponse>();
        result.Success.ShouldBeFalse();
        result.ValidationErrors.ShouldNotBeNull();
        result.ValidationErrors.ShouldContain("Category Code is not exists");
    }

    [Fact]
    public async Task UpdateTransactionTest_NegativeAmount_ShouldSucceed()
    {
        var handler = new UpdateTransactionCommandHandler(
            _mapper,
            _mockTransactionRepository.Object,
            _mockTransactionCategoryRepository.Object);

        var existingTransactionId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
        var command = new UpdateTransactionCommand
        {
            Id = existingTransactionId,
            Description = "Refund Transaction",
            Amount = -75.50,
            ExpenseDate = DateTime.Now,
            CategoryCode = "FOOD_001"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldBeOfType<TransactionResponse>();
        result.Success.ShouldBeTrue();
        result.Transaction.Amount.ShouldBe(command.Amount);
    }

    [Fact]
    public async Task UpdateTransactionTest_OnlyDescriptionChange_ShouldSucceed()
    {
        var handler = new UpdateTransactionCommandHandler(
            _mapper,
            _mockTransactionRepository.Object,
            _mockTransactionCategoryRepository.Object
        );

        var existingTransactionId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
        var command = new UpdateTransactionCommand
        {
            Id = existingTransactionId,
            Description = "Only Description Changed",
            Amount = 100.0, // Keep original amount
            ExpenseDate = DateTime.Parse("2023-01-01"), // Keep original date
            CategoryCode = "FOOD_001" // Keep original category
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldBeOfType<TransactionResponse>();
        result.Success.ShouldBeTrue();
        result.Transaction.Description.ShouldBe("Only Description Changed");
    }
}