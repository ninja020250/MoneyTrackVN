using Microsoft.EntityFrameworkCore;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Persistence;

public class MoneyTrackDbContext : DbContext
{
    public MoneyTrackDbContext(DbContextOptions<MoneyTrackDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; set; }

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
                    entry.Entity.CreatedDate = DateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedDate = DateTime.Now;
                    break;
            }

        return base.SaveChangesAsync(cancellationToken);
    }
}