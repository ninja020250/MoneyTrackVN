# MoneyTrack

A financial tracking application built with .NET 9 following Clean Architecture principles.

## 🏗️ Clean Architecture Overview

This project implements Clean Architecture with clear separation of concerns across multiple layers:

```
┌─────────────────────────────────────────────────────────────┐
│                        API Layer                           │
│                   (MoneyTrack.Api)                        │
└─────────────────────┬───────────────────────────────────────┘
                      │
┌─────────────────────▼───────────────────────────────────────┐
│                  Application Layer                         │
│                (MoneyTrack.Application)                   │
└─────────────────────┬───────────────────────────────────────┘
                      │
┌─────────────────────▼───────────────────────────────────────┐
│                   Domain Layer                             │
│                 (MoneyTrack.Domain)                       │
└─────────────────────▲───────────────────────────────────────┘
                      │
┌─────────────────────┴───────────────┬───────────────────────┐
│          Infrastructure Layer       │    Persistence Layer │
│      (MoneyTrack.Infrastructure)    │  (MoneyTrack.Persistence)│
└─────────────────────────────────────┴───────────────────────┘
```

## 📁 Project Structure

### 🎯 Core Layers

#### **MoneyTrack.Domain**

- **Purpose**: Contains enterprise business rules and entities
- **Dependencies**: None (Pure .NET)
- **Key Components**:
  - `Entities/`: Domain entities with business logic
    - `UserEntity.cs`: User domain model with audit capabilities
    - `AuditableEntity.cs`: Base entity for audit tracking

#### **MoneyTrack.Application**

- **Purpose**: Contains application business rules and use cases
- **Dependencies**: Domain layer only
- **Key Components**:
  - `Contracts/`: Interface definitions for external dependencies
    - `Infrastructure/`: Infrastructure service contracts
    - `Persistence/`: Data access contracts
  - `Features/`: Feature-based organization (CQRS pattern ready)
  - `Exceptions/`: Custom application exceptions
  - `Models/`: Application-specific models
  - `Profiles/`: AutoMapper configuration
  - `Responses/`: Standardized response models
- **Technologies**:
  - MediatR (CQRS/Mediator pattern)
  - AutoMapper (Object mapping)
  - FluentValidation (Input validation)

### 🔌 Infrastructure Layers

#### **MoneyTrack.Persistence**

- **Purpose**: Data access implementation
- **Dependencies**: Application and Domain layers
- **Key Components**:
  - `MoneyTrackDbContext.cs`: Entity Framework DbContext
  - `Repositories/`: Repository pattern implementation
    - `BaseRepository.cs`: Generic repository base
    - `UserRepository.cs`: User-specific data operations
  - `Configurations/`: Entity Framework configurations
  - `Migrations/`: Database migration files
- **Technologies**:
  - Entity Framework Core 9.0
  - PostgreSQL (Npgsql provider)
  - Repository pattern

#### **MoneyTrack.Infrastructure**

- **Purpose**: External service implementations
- **Dependencies**: Application layer
- **Key Components**:
  - `Mail/`: Email service implementation
    - `EmailService.cs`: Email functionality
- **Technologies**:
  - Email services
  - External API integrations

### 🌐 Presentation Layer

#### **MoneyTrack.Api**

- **Purpose**: Web API endpoints and HTTP concerns
- **Dependencies**: Application, Infrastructure, and Persistence layers
- **Key Components**:
  - `Controllers/`: API controllers
    - `UserController.cs`: User management endpoints
  - `Program.cs`: Application entry point
  - `StartupExtensions.cs`: Service configuration and middleware setup
- **Technologies**:
  - ASP.NET Core 9.0
  - RESTful API design
  - CORS configuration

## 🛠️ Technology Stack

### **Framework & Runtime**

- .NET 9.0
- ASP.NET Core 9.0

### **Database**

- PostgreSQL
- Entity Framework Core 9.0

### **Architecture Patterns**

- Clean Architecture
- Repository Pattern
- CQRS (via MediatR)
- Dependency Injection

### **Key Libraries**

- **MediatR**: Mediator pattern implementation
- **AutoMapper**: Object-to-object mapping
- **FluentValidation**: Input validation
- **Npgsql**: PostgreSQL .NET driver

## 🚀 Getting Started

### Prerequisites

- .NET 9.0 SDK
- PostgreSQL database

### Configuration

The application supports both local and environment-based database configuration:

**Local Development** (appsettings.json):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your PostgreSQL connection string"
  }
}
```

**Production** (Environment Variables):

- `DB_HOST`: Database host
- `DB_PORT`: Database port
- `DB_DATABASE`: Database name
- `DB_USERNAME`: Database username
- `DB_PASSWORD`: Database password

### Running the Application

1. **Clone the repository**
2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```
3. **Update database**:
   ```bash
   dotnet ef database update --project MoneyTrack.Persistence --startup-project MoneyTrack.Api
   ```
4. **Run the application**:
   ```bash
   dotnet run --project MoneyTrack.Api
   ```

## 🏛️ Clean Architecture Benefits

### **Dependency Rule**

- Dependencies point inward toward the domain
- Domain layer has no external dependencies
- Infrastructure depends on abstractions, not concretions

### **Testability**

- Business logic isolated in Domain and Application layers
- Infrastructure concerns mocked via interfaces
- Clear separation enables comprehensive unit testing

### **Maintainability**

- Feature-based organization
- Single Responsibility Principle
- Open/Closed Principle through dependency injection

### **Flexibility**

- Easy to swap infrastructure implementations
- Database-agnostic business logic
- Framework-independent core logic

## 📋 Features

- User management with audit tracking
- Email service integration
- Database migrations
- CORS support for frontend integration
- Environment-based configuration
- Automatic database reset for development

## 🔄 Development Workflow

1. **Domain First**: Define entities and business rules
2. **Application Layer**: Implement use cases and contracts
3. **Infrastructure**: Implement external concerns
4. **API Layer**: Expose functionality via HTTP endpoints

This structure ensures that business logic remains pure and testable while keeping infrastructure concerns properly separated.

## 📂 Detailed Folder Structure

```
MoneyTrack/
├── MoneyTrack.Api/                    # 🌐 Presentation Layer
│   ├── Controllers/                   # API Controllers
│   │   └── UserController.cs
│   ├── Program.cs                     # Application entry point
│   ├── StartupExtensions.cs           # Service configuration
│   ├── appsettings.json              # Configuration files
│   └── appsettings.Development.json
│
├── MoneyTrack.Application/            # 🎯 Application Layer
│   ├── Contracts/                     # Interface definitions
│   │   ├── Infrastructure/            # Infrastructure contracts
│   │   └── Persistence/              # Data access contracts
│   ├── Features/                      # Feature organization
│   │   └── Users/                    # User-related features
│   ├── Exceptions/                    # Custom exceptions
│   │   ├── BadRequestException.cs
│   │   ├── NotFoundException.cs
│   │   └── ValidationException.cs
│   ├── Models/                        # Application models
│   │   └── Mail/                     # Email models
│   ├── Profiles/                      # AutoMapper profiles
│   │   └── MappingProfile.cs
│   ├── Responses/                     # Response models
│   │   └── BaseResponse.cs
│   └── ApplicationServiceRegistration.cs
│
├── MoneyTrack.Domain/                 # 🎯 Domain Layer
│   └── Entities/                      # Domain entities
│       ├── AuditableEntity.cs        # Base audit entity
│       └── UserEntity.cs             # User domain model
│
├── MoneyTrack.Infrastructure/         # 🔌 Infrastructure Layer
│   ├── Mail/                          # Email services
│   │   └── EmailService.cs
│   └── InfrastructureServiceRegistration.cs
│
├── MoneyTrack.Persistence/            # 🔌 Persistence Layer
│   ├── Configurations/                # EF configurations
│   │   └── UserConfiguration.cs
│   ├── Migrations/                    # Database migrations
│   │   ├── 20250805073208_Initial.cs
│   │   └── ...
│   ├── Repositories/                  # Repository implementations
│   │   ├── BaseRepository.cs         # Generic repository
│   │   └── UserRepository.cs         # User repository
│   ├── MoneyTrackDbContext.cs        # EF DbContext
│   └── PersistenceServiceRegistration.cs
│
└── MoneyTrack.sln                     # Solution file
```

## 🧪 Testing Strategy

### Unit Tests

- **Domain Layer**: Test business logic and entity behavior
- **Application Layer**: Test use cases and business rules
- **Infrastructure Layer**: Test external service integrations

### Integration Tests

- **API Layer**: Test HTTP endpoints and request/response flow
- **Persistence Layer**: Test database operations and migrations

### Architecture Tests

- Verify dependency rules are not violated
- Ensure proper layer separation
- Validate naming conventions

## 🔧 Development Guidelines

### Adding New Features

1. **Start with Domain**: Define entities and business rules
2. **Application Contracts**: Define interfaces for external dependencies
3. **Application Logic**: Implement use cases using MediatR
4. **Infrastructure**: Implement external service integrations
5. **Persistence**: Add repository methods and configurations
6. **API**: Create controllers and endpoints

### Code Organization

- Use feature folders in Application layer
- Follow CQRS pattern with MediatR
- Implement proper validation using FluentValidation
- Use AutoMapper for object mapping
- Follow repository pattern for data access

## 📝 Contributing

1. Follow Clean Architecture principles
2. Maintain proper layer separation
3. Write comprehensive tests
4. Use consistent naming conventions
5. Document public APIs

## 📄 License

[Add your license information here]
