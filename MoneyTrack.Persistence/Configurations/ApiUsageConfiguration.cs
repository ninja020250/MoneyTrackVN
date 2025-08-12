using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Persistence.Configurations;

public class ApiUsageConfiguration : IEntityTypeConfiguration<ApiUsageEntity>
{
    public void Configure(EntityTypeBuilder<ApiUsageEntity> builder)
    {
        builder.ToTable("api_usage");
        builder.HasIndex(apiUsage => new { apiUsage.UserId, apiUsage.ApiName, apiUsage.CallDate })
            .IsUnique();

        builder.HasOne(apiUsage => apiUsage.User)
            .WithMany(user => user.apiUsages)
            .HasForeignKey(apiUsage => apiUsage.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}