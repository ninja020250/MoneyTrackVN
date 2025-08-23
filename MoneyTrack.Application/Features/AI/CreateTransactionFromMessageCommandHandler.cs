using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Application.Features.Transactions.Queries;
using MoneyTrack.Application.Models.Category;
using MoneyTrack.Application.Models.Transaction;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Application.Features.AI;

public class
    CreateTransactionFromMessageCommandHandler : IRequestHandler<CreateTransactionFromMessageCommand,
    CreateTransactionFromMessageCommandResponse>
{
    private readonly ILLMService _llmService;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionCategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateTransactionFromMessageCommandHandler> _logger;

    public CreateTransactionFromMessageCommandHandler(
        ILLMService llmService,
        ITransactionRepository transactionRepository,
        ITransactionCategoryRepository categoryRepository,
        IMapper mapper,
        ILogger<CreateTransactionFromMessageCommandHandler> logger)
    {
        _llmService = llmService;
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CreateTransactionFromMessageCommandResponse> Handle(CreateTransactionFromMessageCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Processing AI transaction creation for user {UserId} with message: {Message}",
                request.UserId, request.Message);

            var categories = await _categoryRepository.ListAllAsync();

            // Parse message using Gemini AI
            var aiTransactionDto = await _llmService.ParseTransactionAsync(
                message: request.Message,
                language: "Vietnamese",
                currencyUnit: "VND",
                categories: categories
            );

            // Validate and get category
            var category = categories.ToList().Find(c => c.Code == aiTransactionDto.CategoryCode);
            if (category == null)
            {
                _logger.LogWarning("Category {CategoryCode} not found, using default", aiTransactionDto.CategoryCode);
                return new CreateTransactionFromMessageCommandResponse
                {
                    Success = false,
                    Message = "Can not find suitable category",
                };
            }

            // // Map to DTO
            var transactionDto = new GetTransactionDto()
            {
                Id = Guid.NewGuid(),
                Description = aiTransactionDto.Description,
                Amount = aiTransactionDto.Amount,
                ExpenseDate = aiTransactionDto.ExpenseDate,
            };
            transactionDto.Category = _mapper.Map<GetCategoryDto>(category);

            return new CreateTransactionFromMessageCommandResponse
            {
                Success = true,
                Message = "Transaction created successfully from AI parsing. But haven't created a transaction",
                transaction = aiTransactionDto,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating transaction from message for user {UserId}", request.UserId);
            return new CreateTransactionFromMessageCommandResponse
            {
                Success = false,
                Message = "Failed to create transaction from message. Please try again or create manually.",
            };
        }
    }
}