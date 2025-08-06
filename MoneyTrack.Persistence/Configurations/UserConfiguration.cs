using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("user");
        
        builder.HasKey(u => u.Id); // Primary key
        builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
        builder.Property(u => u.Username).IsRequired().HasMaxLength(255);
    }
}