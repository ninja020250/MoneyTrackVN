using AutoMapper;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Exceptions;
using MoneyTrack.Application.Features.Transactions.Commands;
using MoneyTrack.Application.Models;
using MoneyTrack.Application.Profiles;
using MoneyTrack.Application.Responses;
using MoneyTrack.Application.UnitTests.Mocks;
using MoneyTrack.Domain.Entities;
using Moq;
using Shouldly;

namespace MoneyTrack.Application.UnitTests.Transactions.Commands;

public class DeleteTransactionCommandHandlerTest
{
    private readonly IMapper _mapper;
    private readonly Mock<ITransactionRepository> _mockTransactionRepository;

    public DeleteTransactionCommandHandlerTest()
    {
        _mockTransactionRepository = RepositoryMocks.GetTransactionRepository();
        var configurationProvider = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });
        _mapper = configurationProvider.CreateMapper();
    }

    [Fact]
    public async Task DeleteTransactionTest_Success()
    {
        var handler = new DeleteTransactionCommandHandler(
            _mockTransactionRepository.Object);

        var existingTransactionId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
        var command = new DeleteTransactionCommand
        {
            Id = existingTransactionId
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldBeOfType<BaseResponse>();
        result.Success.ShouldBeTrue();
    }

    [Fact]
    public async Task DeleteTransactionTest_NonExistentTransaction_ShouldFail()
    {
        var handler = new DeleteTransactionCommandHandler(
            _mockTransactionRepository.Object);

        var nonExistentId = Guid.NewGuid();
        var command = new DeleteTransactionCommand
        {
            Id = nonExistentId
        };

        var exception = await Should.ThrowAsync<NotFoundException>(
            async () => await handler.Handle(command, CancellationToken.None));

        exception.ShouldNotBeNull();
    }

    [Fact]
    public async Task DeleteTransactionTest_EmptyGuid_ShouldFail()
    {
        var handler = new DeleteTransactionCommandHandler(
            _mockTransactionRepository.Object);

        var command = new DeleteTransactionCommand
        {
            Id = Guid.Empty
        };

        var exception = await Should.ThrowAsync<Exception>(
            async () => await handler.Handle(command, CancellationToken.None));

        exception.ShouldNotBeNull();
    }

    [Fact]
    public async Task DeleteTransactionTest_ValidId_ShouldCallRepositoryDelete()
    {
        var handler = new DeleteTransactionCommandHandler(
            _mockTransactionRepository.Object);

        var existingTransactionId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");
        var command = new DeleteTransactionCommand
        {
            Id = existingTransactionId
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.Success.ShouldBeTrue();
        _mockTransactionRepository.Verify(
            x => x.GetByIdAsync(existingTransactionId),
            Times.Once);
        _mockTransactionRepository.Verify(
            x => x.DeleteAsync(It.IsAny<TransactionEntity>()),
            Times.Once);
    }
}