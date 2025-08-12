# MoneyTrack - Technical Architecture

## 🏗️ Clean Architecture Overview

MoneyTrack follows **Clean Architecture** principles, ensuring separation of concerns, testability, and maintainability. The architecture is designed to be independent of frameworks, UI, databases, and external agencies.

### Architecture Layers

```
┌─────────────────────────────────────────────────────────────┐
│                        🌐 API Layer                        │
│                    (Controllers, DTOs)                     │
├─────────────────────────────────────────────────────────────┤
│                   🔌 Infrastructure Layer                  │
│              (External Services, Email, AI)                │
├─────────────────────────────────────────────────────────────┤
│                    💾 Persistence Layer                    │
│                (EF Core, Repositories)                     │
├─────────────────────────────────────────────────────────────┤
│                   🎯 Application Layer                     │
│              (Use Cases, Commands, Queries)                │
├─────────────────────────────────────────────────────────────┤
│                     🏛️ Domain Layer                        │
│                 (Entities, Value Objects)                  │
└─────────────────────────────────────────────────────────────┘
```

### Dependency Flow

- **Outer layers depend on inner layers**
- **Inner layers are independent of outer layers**
- **Domain layer has no dependencies**
- **Application layer depends only on Domain**

## 📁 Project Structure

### Detailed Folder Organization

```
MoneyTrack/
├── 🌐 MoneyTrack.Api/                    # Web API Layer
│   ├── Controllers/                       # API Controllers
│   │   ├── AuthController.cs             # Authentication endpoints
│   │   ├── TransactionsController.cs     # Transaction management
│   │   ├── UsersController.cs            # User management
│   │   └── HealthController.cs           # Health check endpoints
│   ├── DTOs/                             # Data Transfer Objects
│   │   ├── Requests/                     # Request DTOs
│   │   └── Responses/                    # Response DTOs
│   ├── Middleware/                       # Custom middleware
│   │   ├── ExceptionHandlingMiddleware.cs
│   │   ├── AuthenticationMiddleware.cs
│   │   └── LoggingMiddleware.cs
│   ├── Filters/                          # Action filters
│   ├── Extensions/                       # Service extensions
│   │   ├── ServiceCollectionExtensions.cs
│   │   └── ApplicationBuilderExtensions.cs
│   ├── appsettings.json                  # Configuration
│   ├── appsettings.Development.json      # Dev configuration
│   ├── Program.cs                        # Application entry point
│   └── Startup.cs                        # Service configuration
│
├── 🎯 MoneyTrack.Application/            # Business Logic Layer
│   ├── Features/                         # Feature-based organization
│   │   ├── Auth/                         # Authentication features
│   │   │   ├── Commands/                 # Auth commands
│   │   │   ├── Queries/                  # Auth queries
│   │   │   └── Handlers/                 # Command/Query handlers
│   │   ├── Transactions/                 # Transaction features
│   │   │   ├── Commands/
│   │   │   │   ├── CreateTransactionCommand.cs
│   │   │   │   ├── UpdateTransactionCommand.cs
│   │   │   │   └── DeleteTransactionCommand.cs
│   │   │   ├── Queries/
│   │   │   │   ├── GetTransactionQuery.cs
│   │   │   │   └── GetTransactionsQuery.cs
│   │   │   └── Handlers/
│   │   │       ├── CreateTransactionCommandHandler.cs
│   │   │       └── GetTransactionQueryHandler.cs
│   │   ├── Users/                        # User management features
│   │   └── AI/                           # AI-powered features
│   │       ├── Commands/
│   │       │   └── CreateTransactionFromMessageCommand.cs
│   │       └── Handlers/
│   │           └── CreateTransactionFromMessageCommandHandler.cs
│   ├── Common/                           # Shared application logic
│   │   ├── Behaviors/                    # MediatR behaviors
│   │   │   ├── ValidationBehavior.cs    # Request validation
│   │   │   ├── LoggingBehavior.cs       # Request logging
│   │   │   └── PerformanceBehavior.cs   # Performance monitoring
│   │   ├── Exceptions/                   # Custom exceptions
│   │   ├── Mappings/                     # AutoMapper profiles
│   │   └── Validators/                   # FluentValidation validators
│   ├── Contracts/                        # Interface definitions
│   │   ├── Persistence/                  # Data access interfaces
│   │   │   ├── IApplicationDbContext.cs
│   │   │   ├── ITransactionRepository.cs
│   │   │   └── IUserRepository.cs
│   │   └── Infrastructure/               # External service interfaces
│   │       ├── IEmailService.cs
│   │       ├── IGeminiService.cs
│   │       └── IDateTimeProvider.cs
│   └── DependencyInjection.cs            # Service registration
│
├── 🏛️ MoneyTrack.Domain/                 # Core Domain Layer
│   ├── Entities/                         # Domain entities
│   │   ├── User.cs                       # User aggregate root
│   │   ├── Transaction.cs                # Transaction entity
│   │   ├── Category.cs                   # Category entity
│   │   └── Budget.cs                     # Budget entity
│   ├── ValueObjects/                     # Value objects
│   │   ├── Money.cs                      # Money value object
│   │   ├── Email.cs                      # Email value object
│   │   └── TransactionDate.cs            # Date value object
│   ├── Enums/                            # Domain enumerations
│   │   ├── TransactionType.cs            # Income/Expense types
│   │   ├── UserRole.cs                   # User roles
│   │   └── TransactionStatus.cs          # Transaction statuses
│   ├── Events/                           # Domain events
│   │   ├── TransactionCreatedEvent.cs
│   │   ├── UserRegisteredEvent.cs
│   │   └── BudgetExceededEvent.cs
│   ├── Exceptions/                       # Domain exceptions
│   │   ├── DomainException.cs
│   │   ├── InvalidTransactionException.cs
│   │   └── UserNotFoundException.cs
│   └── Common/                           # Shared domain logic
│       ├── BaseEntity.cs                 # Base entity class
│       ├── IAggregateRoot.cs             # Aggregate root marker
│       └── IDomainEvent.cs               # Domain event interface
│
├── 💾 MoneyTrack.Persistence/            # Data Access Layer
│   ├── Context/                          # Database context
│   │   ├── ApplicationDbContext.cs       # Main EF Core context
│   │   └── ApplicationDbContextFactory.cs # Design-time factory
│   ├── Configurations/                   # Entity configurations
│   │   ├── UserConfiguration.cs          # User entity config
│   │   ├── TransactionConfiguration.cs   # Transaction entity config
│   │   └── CategoryConfiguration.cs      # Category entity config
│   ├── Repositories/                     # Repository implementations
│   │   ├── TransactionRepository.cs      # Transaction data access
│   │   ├── UserRepository.cs             # User data access
│   │   └── CategoryRepository.cs         # Category data access
│   ├── Migrations/                       # EF Core migrations
│   │   ├── 20240101000000_InitialCreate.cs
│   │   ├── 20240102000000_AddCategories.cs
│   │   └── 20240103000000_AddBudgets.cs
│   ├── Seeders/                          # Data seeders
│   │   ├── UserSeeder.cs                 # Initial user data
│   │   └── CategorySeeder.cs             # Default categories
│   └── DependencyInjection.cs            # Persistence services
│
├── 🔌 MoneyTrack.Infrastructure/         # External Services Layer
│   ├── AI/                               # AI service implementations
│   │   ├── GeminiService.cs              # Google Gemini integration
│   │   ├── GeminiSettings.cs             # Gemini configuration
│   │   └── Adapters/                     # AI service adapters
│   │       └── IGeminiLLMService.cs      # LLM service interface
│   ├── Email/                            # Email service implementations
│   │   ├── EmailService.cs               # SMTP email service
│   │   ├── EmailSettings.cs              # Email configuration
│   │   └── Templates/                    # Email templates
│   │       ├── WelcomeEmailTemplate.cs
│   │       └── TransactionSummaryTemplate.cs
│   ├── Authentication/                   # Auth implementations
│   │   ├── JwtTokenService.cs            # JWT token generation
│   │   ├── PasswordHasher.cs             # Password hashing
│   │   └── AuthSettings.cs               # Auth configuration
│   ├── Monitoring/                       # Monitoring services
│   │   ├── HealthChecks/                 # Health check implementations
│   │   │   ├── DatabaseHealthCheck.cs
│   │   │   ├── GeminiHealthCheck.cs
│   │   │   └── EmailHealthCheck.cs
│   │   └── Logging/                      # Logging implementations
│   │       └── SerilogConfiguration.cs
│   ├── Common/                           # Shared infrastructure
│   │   ├── DateTimeProvider.cs           # System time provider
│   │   └── FileService.cs                # File operations
│   └── DependencyInjection.cs            # Infrastructure services
│
└── 🧪 MoneyTrack.Tests/                  # Test Projects
    ├── MoneyTrack.Domain.Tests/          # Domain layer tests
    │   ├── Entities/                     # Entity tests
    │   ├── ValueObjects/                 # Value object tests
    │   └── Events/                       # Domain event tests
    ├── MoneyTrack.Application.Tests/     # Application layer tests
    │   ├── Features/                     # Feature tests
    │   ├── Behaviors/                    # Behavior tests
    │   └── Validators/                   # Validation tests
    ├── MoneyTrack.Infrastructure.Tests/  # Infrastructure tests
    │   ├── AI/                           # AI service tests
    │   ├── Email/                        # Email service tests
    │   └── Authentication/               # Auth service tests
    ├── MoneyTrack.Persistence.Tests/     # Persistence tests
    │   ├── Repositories/                 # Repository tests
    │   └── Context/                      # DbContext tests
    ├── MoneyTrack.Api.Tests/             # API layer tests
    │   ├── Controllers/                  # Controller tests
    │   ├── Middleware/                   # Middleware tests
    │   └── Integration/                  # Integration tests
    └── Common/                           # Shared test utilities
        ├── Fixtures/                     # Test fixtures
        ├── Builders/                     # Test data builders
        └── Helpers/                      # Test helpers
```

## 🛠️ Technology Stack

### Core Framework

| Component | Technology | Version | Purpose |
|-----------|------------|---------|----------|
| **Runtime** | .NET | 9.0 | Application runtime |
| **Framework** | ASP.NET Core | 9.0 | Web API framework |
| **Language** | C# | 12.0 | Programming language |

### Architecture Patterns

| Pattern | Implementation | Purpose |
|---------|----------------|----------|
| **Clean Architecture** | Layered structure | Separation of concerns |
| **CQRS** | MediatR | Command/Query separation |
| **Repository Pattern** | Custom repositories | Data access abstraction |
| **Unit of Work** | EF Core DbContext | Transaction management |
| **Domain Events** | MediatR notifications | Domain event handling |

### Data & Persistence

| Component | Technology | Purpose |
|-----------|------------|----------|
| **Database** | PostgreSQL | Primary data store |
| **ORM** | Entity Framework Core | Object-relational mapping |
| **Migrations** | EF Core Migrations | Database schema versioning |
| **Connection Pooling** | Npgsql | Database connection management |

### External Services

| Service | Technology | Purpose |
|---------|------------|----------|
| **AI/ML** | Google Gemini API | Natural language processing |
| **Email** | SMTP | Email notifications |
| **Authentication** | JWT | Token-based authentication |
| **Logging** | Serilog | Structured logging |

### Development & Testing

| Component | Technology | Purpose |
|-----------|------------|----------|
| **Testing Framework** | xUnit | Unit testing |
| **Mocking** | Moq | Test doubles |
| **Validation** | FluentValidation | Input validation |
| **Mapping** | AutoMapper | Object mapping |
| **API Documentation** | Swagger/OpenAPI | API documentation |

## 🎯 Design Patterns

### 1. Command Query Responsibility Segregation (CQRS)

```csharp
// Command Example
public class CreateTransactionCommand : IRequest<TransactionDto>
{
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public TransactionType Type { get; set; }
}

// Query Example
public class GetTransactionQuery : IRequest<TransactionDto>
{
    public Guid TransactionId { get; set; }
}

// Handler Example
public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, TransactionDto>
{
    private readonly ITransactionRepository _repository;
    
    public async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        // Business logic implementation
    }
}
```

### 2. Repository Pattern

```csharp
// Interface (Application Layer)
public interface ITransactionRepository
{
    Task<Transaction> GetByIdAsync(Guid id);
    Task<IEnumerable<Transaction>> GetByUserIdAsync(Guid userId);
    Task<Transaction> AddAsync(Transaction transaction);
    Task UpdateAsync(Transaction transaction);
    Task DeleteAsync(Guid id);
}

// Implementation (Persistence Layer)
public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _context;
    
    // Implementation details
}
```

### 3. Domain Events

```csharp
// Domain Event
public class TransactionCreatedEvent : IDomainEvent
{
    public Guid TransactionId { get; }
    public Guid UserId { get; }
    public decimal Amount { get; }
    public DateTime OccurredOn { get; }
}

// Event Handler
public class TransactionCreatedEventHandler : INotificationHandler<TransactionCreatedEvent>
{
    public async Task Handle(TransactionCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Handle the event (e.g., send notification, update statistics)
    }
}
```

### 4. Specification Pattern

```csharp
public class TransactionSpecification
{
    public static Expression<Func<Transaction, bool>> ByUserId(Guid userId)
        => transaction => transaction.UserId == userId;
    
    public static Expression<Func<Transaction, bool>> ByDateRange(DateTime start, DateTime end)
        => transaction => transaction.Date >= start && transaction.Date <= end;
    
    public static Expression<Func<Transaction, bool>> ByType(TransactionType type)
        => transaction => transaction.Type == type;
}
```

### 5. Factory Pattern

```csharp
public interface ITransactionFactory
{
    Transaction CreateExpense(decimal amount, string description, Guid userId);
    Transaction CreateIncome(decimal amount, string description, Guid userId);
}

public class TransactionFactory : ITransactionFactory
{
    public Transaction CreateExpense(decimal amount, string description, Guid userId)
    {
        return new Transaction
        {
            Id = Guid.NewGuid(),
            Amount = amount,
            Description = description,
            Type = TransactionType.Expense,
            UserId = userId,
            Date = DateTime.UtcNow
        };
    }
}
```

## 🗄️ Database Design

### Entity Relationship Diagram

```
┌─────────────────┐     ┌─────────────────┐     ┌─────────────────┐
│      Users      │     │   Transactions  │     │   Categories    │
├─────────────────┤     ├─────────────────┤     ├─────────────────┤
│ Id (PK)         │────<│ UserId (FK)     │>────│ Id (PK)         │
│ Email           │     │ Id (PK)         │     │ Name            │
│ PasswordHash    │     │ Amount          │     │ Description     │
│ FirstName       │     │ Description     │     │ Color           │
│ LastName        │     │ Date            │     │ Icon            │
│ CreatedAt       │     │ Type            │     │ CreatedAt       │
│ UpdatedAt       │     │ CategoryId (FK) │     │ UpdatedAt       │
│ IsActive        │     │ CreatedAt       │     │ IsActive        │
└─────────────────┘     │ UpdatedAt       │     └─────────────────┘
                        │ IsDeleted       │
                        └─────────────────┘
                                │
                                │
                        ┌─────────────────┐
                        │     Budgets     │
                        ├─────────────────┤
                        │ Id (PK)         │
                        │ UserId (FK)     │
                        │ CategoryId (FK) │
                        │ Amount          │
                        │ Period          │
                        │ StartDate       │
                        │ EndDate         │
                        │ CreatedAt       │
                        │ UpdatedAt       │
                        │ IsActive        │
                        └─────────────────┘
```

### Database Constraints

```sql
-- Primary Keys
ALTER TABLE Users ADD CONSTRAINT PK_Users PRIMARY KEY (Id);
ALTER TABLE Transactions ADD CONSTRAINT PK_Transactions PRIMARY KEY (Id);
ALTER TABLE Categories ADD CONSTRAINT PK_Categories PRIMARY KEY (Id);
ALTER TABLE Budgets ADD CONSTRAINT PK_Budgets PRIMARY KEY (Id);

-- Foreign Keys
ALTER TABLE Transactions ADD CONSTRAINT FK_Transactions_Users 
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE;
ALTER TABLE Transactions ADD CONSTRAINT FK_Transactions_Categories 
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE SET NULL;
ALTER TABLE Budgets ADD CONSTRAINT FK_Budgets_Users 
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE;
ALTER TABLE Budgets ADD CONSTRAINT FK_Budgets_Categories 
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE CASCADE;

-- Unique Constraints
ALTER TABLE Users ADD CONSTRAINT UQ_Users_Email UNIQUE (Email);
ALTER TABLE Categories ADD CONSTRAINT UQ_Categories_Name UNIQUE (Name);

-- Check Constraints
ALTER TABLE Transactions ADD CONSTRAINT CK_Transactions_Amount 
    CHECK (Amount > 0);
ALTER TABLE Budgets ADD CONSTRAINT CK_Budgets_Amount 
    CHECK (Amount > 0);
ALTER TABLE Budgets ADD CONSTRAINT CK_Budgets_DateRange 
    CHECK (EndDate > StartDate);
```

### Indexing Strategy

```sql
-- Performance Indexes
CREATE INDEX IX_Transactions_UserId ON Transactions(UserId);
CREATE INDEX IX_Transactions_Date ON Transactions(Date DESC);
CREATE INDEX IX_Transactions_Type ON Transactions(Type);
CREATE INDEX IX_Transactions_CategoryId ON Transactions(CategoryId);
CREATE INDEX IX_Budgets_UserId ON Budgets(UserId);
CREATE INDEX IX_Budgets_Period ON Budgets(StartDate, EndDate);

-- Composite Indexes
CREATE INDEX IX_Transactions_User_Date ON Transactions(UserId, Date DESC);
CREATE INDEX IX_Transactions_User_Type ON Transactions(UserId, Type);
CREATE INDEX IX_Budgets_User_Category ON Budgets(UserId, CategoryId);
```

## 🔧 Configuration Management

### Environment-Based Configuration

```json
// appsettings.json (Base)
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=moneytrack;Username=postgres;Password=password"
  },
  "GeminiSettings": {
    "BaseUrl": "https://generativelanguage.googleapis.com/v1beta",
    "Model": "gemini-pro",
    "MaxTokens": 1000,
    "Temperature": 0.1
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key",
    "Issuer": "MoneyTrack",
    "Audience": "MoneyTrack-Users",
    "ExpirationMinutes": 60
  },
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "FromEmail": "noreply@moneytrack.com",
    "FromName": "MoneyTrack"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

```json
// appsettings.Development.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information"
    }
  },
  "GeminiSettings": {
    "Temperature": 0.2
  }
}
```

```json
// appsettings.Production.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "MoneyTrack": "Information"
    }
  },
  "GeminiSettings": {
    "Temperature": 0.1
  }
}
```

## 🔒 Security Architecture

### Authentication Flow

```
┌─────────────┐    ┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│   Client    │    │     API     │    │    Auth     │    │  Database   │
│ Application │    │ Controller  │    │  Service    │    │             │
└─────────────┘    └─────────────┘    └─────────────┘    └─────────────┘
       │                   │                   │                   │
       │ POST /auth/login  │                   │                   │
       │──────────────────>│                   │                   │
       │                   │ ValidateUser()    │                   │
       │                   │──────────────────>│                   │
       │                   │                   │ GetUser()         │
       │                   │                   │──────────────────>│
       │                   │                   │<──────────────────│
       │                   │ GenerateToken()   │                   │
       │                   │<──────────────────│                   │
       │<──────────────────│                   │                   │
       │                   │                   │                   │
```

### Authorization Policies

```csharp
// Policy-based authorization
services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => 
        policy.RequireRole("Admin"));
    options.AddPolicy("RequireUserRole", policy => 
        policy.RequireRole("User", "Admin"));
    options.AddPolicy("OwnerOrAdmin", policy => 
        policy.Requirements.Add(new OwnerOrAdminRequirement()));
});
```

### Data Protection

```csharp
// Sensitive data encryption
public class PasswordHasher
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, 12);
    }
    
    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}

// JWT Token configuration
public class JwtTokenService
{
    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
```

## 🚀 Performance Optimization

### Caching Strategy

```csharp
// Memory caching for frequently accessed data
services.AddMemoryCache();
services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});

// Cached repository decorator
public class CachedTransactionRepository : ITransactionRepository
{
    private readonly ITransactionRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(15);
    
    public async Task<Transaction> GetByIdAsync(Guid id)
    {
        var cacheKey = $"transaction_{id}";
        
        if (_cache.TryGetValue(cacheKey, out Transaction cachedTransaction))
        {
            return cachedTransaction;
        }
        
        var transaction = await _repository.GetByIdAsync(id);
        
        if (transaction != null)
        {
            _cache.Set(cacheKey, transaction, _cacheDuration);
        }
        
        return transaction;
    }
}
```

### Database Optimization

```csharp
// Query optimization with projections
public async Task<IEnumerable<TransactionSummaryDto>> GetTransactionSummariesAsync(Guid userId)
{
    return await _context.Transactions
        .Where(t => t.UserId == userId && !t.IsDeleted)
        .Select(t => new TransactionSummaryDto
        {
            Id = t.Id,
            Amount = t.Amount,
            Description = t.Description,
            Date = t.Date,
            CategoryName = t.Category.Name
        })
        .OrderByDescending(t => t.Date)
        .ToListAsync();
}

// Bulk operations for better performance
public async Task<int> BulkUpdateTransactionsAsync(IEnumerable<Transaction> transactions)
{
    _context.UpdateRange(transactions);
    return await _context.SaveChangesAsync();
}
```

## 📊 Monitoring & Observability

### Health Checks

```csharp
// Comprehensive health checks
services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("database")
    .AddCheck<GeminiHealthCheck>("gemini-api")
    .AddCheck<EmailHealthCheck>("email-service")
    .AddCheck("memory", () =>
    {
        var allocated = GC.GetTotalMemory(false);
        var threshold = 1024 * 1024 * 1024; // 1GB
        
        return allocated < threshold 
            ? HealthCheckResult.Healthy($"Memory usage: {allocated / 1024 / 1024} MB")
            : HealthCheckResult.Unhealthy($"High memory usage: {allocated / 1024 / 1024} MB");
    });
```

### Structured Logging

```csharp
// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "MoneyTrack")
    .WriteTo.Console()
    .WriteTo.File("logs/moneytrack-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();

// Structured logging in handlers
public class CreateTransactionCommandHandler
{
    private readonly ILogger<CreateTransactionCommandHandler> _logger;
    
    public async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating transaction for user {UserId} with amount {Amount}", 
            request.UserId, request.Amount);
        
        try
        {
            // Business logic
            var transaction = await _repository.AddAsync(newTransaction);
            
            _logger.LogInformation("Transaction {TransactionId} created successfully", 
                transaction.Id);
            
            return _mapper.Map<TransactionDto>(transaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create transaction for user {UserId}", 
                request.UserId);
            throw;
        }
    }
}
```

## 🔄 CI/CD Architecture

### Build Pipeline

```yaml
# .github/workflows/ci.yml
name: CI/CD Pipeline

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main ]

jobs:
  test:
    runs-on: ubuntu-latest
    
    services:
      postgres:
        image: postgres:15
        env:
          POSTGRES_PASSWORD: postgres
          POSTGRES_DB: moneytrack_test
        options: >-
          --health-cmd pg_isready
          --health-interval 10s
          --health-timeout 5s
          --health-retries 5
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '9.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore
    
    - name: Test
      run: dotnet test --no-build --verbosity normal --collect:"XPlat Code Coverage"
    
    - name: Upload coverage reports
      uses: codecov/codecov-action@v3
```

### Deployment Strategy

```yaml
# Docker deployment
version: '3.8'

services:
  api:
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "8080:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Host=db;Database=moneytrack;Username=postgres;Password=password
    depends_on:
      - db
      - redis
  
  db:
    image: postgres:15
    environment:
      POSTGRES_DB: moneytrack
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: password
    volumes:
      - postgres_data:/var/lib/postgresql/data
    ports:
      - "5432:5432"
  
  redis:
    image: redis:7-alpine
    ports:
      - "6379:6379"

volumes:
  postgres_data:
```

## 🎯 Clean Architecture Benefits

### 1. **Independence**
- **Framework Independence**: Not tied to specific frameworks
- **Database Independence**: Can switch databases without affecting business logic
- **UI Independence**: Business logic doesn't depend on UI
- **External Agency Independence**: Business rules don't know about external services

### 2. **Testability**
- **Unit Testing**: Each layer can be tested in isolation
- **Integration Testing**: Clear boundaries for integration tests
- **Mocking**: Easy to mock dependencies
- **Test Coverage**: High test coverage achievable

### 3. **Maintainability**
- **Separation of Concerns**: Each layer has a single responsibility
- **Loose Coupling**: Minimal dependencies between layers
- **High Cohesion**: Related functionality grouped together
- **Clear Boundaries**: Well-defined interfaces between layers

### 4. **Scalability**
- **Horizontal Scaling**: Stateless design enables scaling
- **Microservices Ready**: Easy to extract services
- **Performance Optimization**: Clear points for optimization
- **Resource Management**: Efficient resource utilization

---

## 📚 Additional Resources

- **Clean Architecture**: [Robert C. Martin's Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- **Domain-Driven Design**: [Eric Evans' DDD](https://domainlanguage.com/)
- **CQRS Pattern**: [Martin Fowler's CQRS](https://martinfowler.com/bliki/CQRS.html)
- **Entity Framework Core**: [Microsoft Documentation](https://docs.microsoft.com/en-us/ef/core/)
- **ASP.NET Core**: [Microsoft Documentation](https://docs.microsoft.com/en-us/aspnet/core/)

---

*This architecture ensures MoneyTrack remains maintainable, testable, and scalable as it grows.* 🚀