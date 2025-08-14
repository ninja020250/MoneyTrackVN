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

public class CreateTransactionCommandHandlerTest
{
    private readonly IMapper _mapper;
    private readonly Mock<ITransactionRepository> _mockTransactionRepository;
    private readonly Mock<ITransactionCategoryRepository> _mockTransactionCategoryRepository;

    public CreateTransactionCommandHandlerTest()
    {
        _mockTransactionRepository = RepositoryMocks.GetTransactionRepository();
        _mockTransactionCategoryRepository = RepositoryMocks.GetTransactionCategoryRepository();
        var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });
        _mapper = configurationProvider.CreateMapper();
    }

    [Fact]
    public async Task CreateTransactionTest_Success()
    {
        var handler = new CreateTransactionCommandHandler(
            _mockTransactionRepository.Object,
            _mockTransactionCategoryRepository.Object,
            _mapper);

        var command = new CreateTransactionCommand
        {
            Description = "Test Transaction",
            Amount = 100.50,
            ExpenseDate = DateTime.Now,
            UserId = Guid.NewGuid(),
            CategoryCode = "FOOD_001"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldBeOfType<TransactionResponse>();
        result.Success.ShouldBeTrue();
        result.Transaction.ShouldNotBeNull();
        result.Transaction.Description.ShouldBe(command.Description);
        result.Transaction.Amount.ShouldBe(command.Amount);
        result.Transaction.Category.Code.ShouldBe(command.CategoryCode);
    }

    [Fact]
    public async Task CreateTransactionTest_InvalidCategory_ShouldFail()
    {
        var handler = new CreateTransactionCommandHandler(
            _mockTransactionRepository.Object,
            _mockTransactionCategoryRepository.Object,
            _mapper);

        var command = new CreateTransactionCommand
        {
            Description = "Test Transaction",
            Amount = 100.50,
            ExpenseDate = DateTime.Now,
            UserId = Guid.NewGuid(),
            CategoryCode = "INVALID_CODE"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.Success.ShouldBeFalse();
        result.ValidationErrors.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task CreateTransactionTest_NegativeAmount_ShouldCreateTransaction()
    {
        var handler = new CreateTransactionCommandHandler(
            _mockTransactionRepository.Object,
            _mockTransactionCategoryRepository.Object,
            _mapper);

        var command = new CreateTransactionCommand
        {
            Description = "Refund Transaction",
            Amount = -50.25,
            ExpenseDate = DateTime.Now,
            UserId = Guid.NewGuid(),
            CategoryCode = "FOOD_001"
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldBeOfType<TransactionResponse>();
        result.Success.ShouldBeTrue();
        result.Transaction.Amount.ShouldBe(command.Amount);
    }
}