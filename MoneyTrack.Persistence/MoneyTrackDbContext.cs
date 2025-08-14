using Microsoft.EntityFrameworkCore;
using MoneyTrack.Application.Contracts.Persistence;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Persistence;

public class MoneyTrackDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;
    
    public MoneyTrackDbContext(DbContextOptions<MoneyTrackDbContext> options) : base(options)
    {
    }
    
    public MoneyTrackDbContext(DbContextOptions<MoneyTrackDbContext> options, ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
    }

    public DbSet<UserEntity> Users { get; set; }

    public DbSet<RoleEntity> Roles { get; set; }

    public DbSet<TransactionCategoryEntity> TransactionCategory { get; set; }

    public DbSet<TransactionEntity> Transactions { get; set; }
    
    public DbSet<ApiUsageEntity> apiUsage { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MoneyTrackDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    entry.Entity.CreatedBy = _currentUserService.Email;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedDate = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = _currentUserService.Email;
                    break;
            }

        return base.SaveChangesAsync(cancellationToken);
    }
}