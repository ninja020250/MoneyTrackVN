using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyTrack.Application.Contracts.Infrastructure;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Persistence.Repositories;

namespace MoneyTrack.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =  configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<MoneyTrackDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ITransactionCategoryRepository, TransactionCategoryRepository>();
        services.AddScoped<IApiUsageRepository, ApiUsageRepository>();

        return services;
    }
}