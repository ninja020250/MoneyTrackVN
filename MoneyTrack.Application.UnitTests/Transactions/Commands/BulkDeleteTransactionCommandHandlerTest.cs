using AutoMapper;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Features.Transactions.Commands;
using MoneyTrack.Application.Models;
using MoneyTrack.Application.Profiles;
using MoneyTrack.Application.Responses;
using MoneyTrack.Application.UnitTests.Mocks;
using MoneyTrack.Domain.Entities;
using Moq;
using Shouldly;

namespace MoneyTrack.Application.UnitTests.Transactions.Commands;

public class BulkDeleteTransactionCommandHandlerTest
{
    private readonly IMapper _mapper;
    private readonly Mock<ITransactionRepository> _mockTransactionRepository;

    public BulkDeleteTransactionCommandHandlerTest()
    {
        _mockTransactionRepository = RepositoryMocks.GetTransactionRepository();
        var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });
        _mapper = configurationProvider.CreateMapper();
    }

    [Fact]
    public async Task BulkDeleteTransactionTest_Success()
    {
        var handler = new BulkDeleteTransactionCommandHandler(
            _mockTransactionRepository.Object);

        var command = new BulkDeleteTransactionCommand
        {
            TransactionIds = new List<Guid>
            {
                Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")
            }
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldBeOfType<BaseResponse>();
        result.Success.ShouldBeTrue();
        result.Message.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task BulkDeleteTransactionTest_EmptyList_ShouldReturnSuccess()
    {
        var handler = new BulkDeleteTransactionCommandHandler(
            _mockTransactionRepository.Object);

        var command = new BulkDeleteTransactionCommand
        {
            TransactionIds = new List<Guid>()
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldBeOfType<BaseResponse>();
        result.Success.ShouldBeTrue();
    }

    [Fact]
    public async Task BulkDeleteTransactionTest_WithNonExistentTransaction_ShouldFail()
    {
        var handler = new BulkDeleteTransactionCommandHandler(
            _mockTransactionRepository.Object);

        var command = new BulkDeleteTransactionCommand
        {
            TransactionIds = new List<Guid>
            {
                Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), // Existing
                Guid.NewGuid() // Non-existent
            }
        };

        var exception = await Should.ThrowAsync<Exception>(
            async () => await handler.Handle(command, CancellationToken.None));

        exception.ShouldNotBeNull();
    }

    [Fact]
    public async Task BulkDeleteTransactionTest_WithEmptyGuids_ShouldFail()
    {
        var handler = new BulkDeleteTransactionCommandHandler(
            _mockTransactionRepository.Object);

        var command = new BulkDeleteTransactionCommand
        {
            TransactionIds = new List<Guid>
            {
                Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                Guid.Empty // Empty GUID
            }
        };

        var exception = await Should.ThrowAsync<Exception>(
            async () => await handler.Handle(command, CancellationToken.None));

        exception.ShouldNotBeNull();
    }

    [Fact]
    public async Task BulkDeleteTransactionTest_LargeList_ShouldSucceed()
    {
        var handler = new BulkDeleteTransactionCommandHandler(
            _mockTransactionRepository.Object);

        var ids = new List<Guid>();
        var baseId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");

        // Add the same existing ID multiple times for simplicity in mock
        for (int i = 0; i < 5; i++)
        {
            ids.Add(baseId);
        }

        var command = new BulkDeleteTransactionCommand
        {
            TransactionIds = ids
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldBeOfType<BaseResponse>();
        result.Success.ShouldBeTrue();
    }

    [Fact]
    public async Task BulkDeleteTransactionTest_ShouldCallRepositoryForEachId()
    {
        var handler = new BulkDeleteTransactionCommandHandler(
            _mockTransactionRepository.Object);

        var command = new BulkDeleteTransactionCommand
        {
            TransactionIds = new List<Guid>
            {
                Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")
            }
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.Success.ShouldBeTrue();
        _mockTransactionRepository.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>()),
            Times.AtLeast(1));
        _mockTransactionRepository.Verify(
            x => x.DeleteAsync(It.IsAny<TransactionEntity>()),
            Times.AtLeast(1));
    }

    [Fact]
    public async Task BulkDeleteTransactionTest_CancellationToken_ShouldRespectCancellation()
    {
        var handler = new BulkDeleteTransactionCommandHandler(
            _mockTransactionRepository.Object);

        var command = new BulkDeleteTransactionCommand
        {
            TransactionIds = new List<Guid>
            {
                Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")
            }
        };

        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        var exception = await Should.ThrowAsync<OperationCanceledException>(
            async () => await handler.Handle(command, cancellationTokenSource.Token));

        exception.ShouldNotBeNull();
    }
}