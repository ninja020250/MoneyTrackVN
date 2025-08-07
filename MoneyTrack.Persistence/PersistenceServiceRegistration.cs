using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Persistence.Repositories;

namespace MoneyTrack.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_HOST") != null
            ? $"Host={Environment.GetEnvironmentVariable("DB_HOST")};Port={Environment.GetEnvironmentVariable("DB_PORT")};Database={Environment.GetEnvironmentVariable("DB_DATABASE")};Username={Environment.GetEnvironmentVariable("DB_USERNAME")};Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};TrustServerCertificate=True"
            : configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<MoneyTrackDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        return services;
    }
}