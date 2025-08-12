# MoneyTrack - Setup & Development Guidelines

## 🚀 Quick Start Guide

### Prerequisites

Before setting up MoneyTrack, ensure you have the following installed:

- **.NET 9.0 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/9.0)
- **PostgreSQL Database** - [Download here](https://www.postgresql.org/download/)
- **Git** - [Download here](https://git-scm.com/downloads)
- **IDE/Editor** - Visual Studio, VS Code, or JetBrains Rider

### System Requirements

| Component | Minimum | Recommended |
|-----------|---------|-------------|
| RAM | 4 GB | 8 GB+ |
| Storage | 2 GB | 5 GB+ |
| OS | Windows 10, macOS 10.15, Ubuntu 18.04 | Latest versions |

## 📥 Installation Steps

### 1. Clone the Repository

```bash
git clone https://github.com/your-username/MoneyTrack.git
cd MoneyTrack
```

### 2. Restore Dependencies

```bash
# Restore all project dependencies
dotnet restore
```

### 3. Database Setup

#### Option A: Local PostgreSQL

1. **Install PostgreSQL** and create a database:
   ```sql
   CREATE DATABASE moneytrack_db;
   CREATE USER moneytrack_user WITH PASSWORD 'your_password';
   GRANT ALL PRIVILEGES ON DATABASE moneytrack_db TO moneytrack_user;
   ```

2. **Update Connection String** in `MoneyTrack.Api/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=moneytrack_db;Username=moneytrack_user;Password=your_password"
     }
   }
   ```

#### Option B: Environment Variables (Production)

Set the following environment variables:

```bash
export DB_HOST=your_db_host
export DB_PORT=5432
export DB_DATABASE=moneytrack_db
export DB_USERNAME=your_username
export DB_PASSWORD=your_password
```

### 4. AI Service Configuration

#### Google Gemini API Setup

1. **Get API Key**:
   - Visit [Google AI Studio](https://makersuite.google.com/app/apikey)
   - Create a new API key
   - Copy the generated key

2. **Configure Settings** in `MoneyTrack.Api/appsettings.json`:
   ```json
   {
     "GeminiSettings": {
       "ApiKey": "your_gemini_api_key",
       "BaseUrl": "https://generativelanguage.googleapis.com/v1beta",
       "Model": "gemini-pro",
       "MaxTokens": 1000,
       "Temperature": 0.1
     }
   }
   ```

3. **Environment Variables** (Production):
   ```bash
   export GEMINI_API_KEY=your_gemini_api_key
   export GEMINI_MODEL=gemini-pro
   ```

### 5. Email Service Configuration (Optional)

```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "your_email@gmail.com",
    "SmtpPassword": "your_app_password",
    "FromEmail": "noreply@moneytrack.com",
    "FromName": "MoneyTrack"
  }
}
```

### 6. Apply Database Migrations

```bash
# Navigate to the API project
cd MoneyTrack.Api

# Apply migrations
dotnet ef database update --project ../MoneyTrack.Persistence
```

## 🏃‍♂️ Running the Application

### Development Mode

```bash
# Run the API server
dotnet run --project MoneyTrack.Api

# Or with hot reload
dotnet watch run --project MoneyTrack.Api
```

The API will be available at:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`
- **Swagger UI**: `https://localhost:5001/swagger`

### Production Mode

```bash
# Build the application
dotnet build --configuration Release

# Publish the application
dotnet publish --configuration Release --output ./publish

# Run the published application
dotnet ./publish/MoneyTrack.Api.dll
```

## 🧪 Testing

### Running Unit Tests

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test MoneyTrack.Application.Tests
```

### API Testing

#### Using Swagger UI
1. Navigate to `https://localhost:5001/swagger`
2. Explore and test API endpoints
3. Use the "Try it out" feature

#### Using curl

```bash
# Health check
curl -X GET "https://localhost:5001/api/health"

# Create AI transaction (requires authentication)
curl -X POST "https://localhost:5001/api/transactions/ai-create" \
  -H "Authorization: Bearer your_jwt_token" \
  -H "Content-Type: application/json" \
  -d '{"message": "I spent $25 on lunch today"}'
```

## 🔧 Development Workflow

### Project Structure Navigation

```
MoneyTrack/
├── MoneyTrack.Api/           # 🌐 Web API Layer
├── MoneyTrack.Application/   # 🎯 Business Logic
├── MoneyTrack.Domain/        # 🏛️ Core Entities
├── MoneyTrack.Infrastructure/# 🔌 External Services
├── MoneyTrack.Persistence/   # 💾 Data Access
└── MoneyTrack.Tests/         # 🧪 Test Projects
```

### Adding New Features

1. **Domain First**: Define entities in `MoneyTrack.Domain`
2. **Application Layer**: Create commands/queries in `MoneyTrack.Application`
3. **Infrastructure**: Implement external services in `MoneyTrack.Infrastructure`
4. **Persistence**: Add repositories in `MoneyTrack.Persistence`
5. **API**: Create controllers in `MoneyTrack.Api`

### Database Migrations

```bash
# Add new migration
dotnet ef migrations add MigrationName --project MoneyTrack.Persistence --startup-project MoneyTrack.Api

# Update database
dotnet ef database update --project MoneyTrack.Persistence --startup-project MoneyTrack.Api

# Remove last migration
dotnet ef migrations remove --project MoneyTrack.Persistence --startup-project MoneyTrack.Api
```

## 🐛 Troubleshooting

### Common Issues

#### Database Connection Issues

```bash
# Check PostgreSQL service status
sudo systemctl status postgresql  # Linux
brew services list | grep postgresql  # macOS

# Test connection
psql -h localhost -U moneytrack_user -d moneytrack_db
```

#### Migration Issues

```bash
# Reset database (development only)
dotnet ef database drop --project MoneyTrack.Persistence --startup-project MoneyTrack.Api
dotnet ef database update --project MoneyTrack.Persistence --startup-project MoneyTrack.Api
```

#### AI Service Issues

1. **Verify API Key**: Check Gemini API key is valid
2. **Check Quotas**: Ensure API quotas are not exceeded
3. **Network Issues**: Verify internet connectivity
4. **Logs**: Check application logs for detailed error messages

### Logging

```bash
# View logs in development
dotnet run --project MoneyTrack.Api --verbosity detailed

# Production logging
# Logs are written to configured sinks (file, console, etc.)
```

## 🚀 Deployment

### Docker Deployment

```dockerfile
# Dockerfile example
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet build --configuration Release
RUN dotnet publish --configuration Release --output /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MoneyTrack.Api.dll"]
```

```bash
# Build and run with Docker
docker build -t moneytrack .
docker run -p 8080:80 moneytrack
```

### Cloud Deployment

#### Azure App Service

```bash
# Deploy to Azure
az webapp up --name moneytrack-app --resource-group moneytrack-rg
```

#### AWS Elastic Beanstalk

```bash
# Package for deployment
dotnet publish --configuration Release
zip -r moneytrack.zip ./bin/Release/net9.0/publish/
```

## 📊 Monitoring & Maintenance

### Health Checks

The application includes built-in health checks:
- **Database**: PostgreSQL connectivity
- **AI Service**: Gemini API availability
- **Email Service**: SMTP connectivity

Access health checks at: `https://localhost:5001/health`

### Performance Monitoring

```bash
# Monitor application performance
dotnet-counters monitor --process-id <pid>

# Memory usage
dotnet-dump collect --process-id <pid>
```

### Backup & Recovery

```bash
# Database backup
pg_dump -h localhost -U moneytrack_user moneytrack_db > backup.sql

# Database restore
psql -h localhost -U moneytrack_user moneytrack_db < backup.sql
```

## 🤝 Contributing

### Code Standards

- Follow Clean Architecture principles
- Use SOLID design principles
- Write comprehensive unit tests
- Follow C# coding conventions
- Document public APIs

### Pull Request Process

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Ensure all tests pass
6. Submit a pull request

### Code Review Checklist

- [ ] Code follows project conventions
- [ ] Tests are included and passing
- [ ] Documentation is updated
- [ ] No breaking changes (or properly documented)
- [ ] Security considerations addressed

---

## 📞 Support

For additional help:
- **Documentation**: Check the `/docs` folder
- **Issues**: Create a GitHub issue
- **Discussions**: Use GitHub Discussions
- **Email**: support@moneytrack.com

---

*Happy coding! 🚀*