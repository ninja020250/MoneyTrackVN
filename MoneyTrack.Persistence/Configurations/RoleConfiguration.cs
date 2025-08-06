using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.ToTable("role");
        builder.HasData(
            new RoleEntity
            {
                Id = new Guid("af2b2a21-21e7-41a2-8727-c67816796132"), // Static Guid
                Name = RoleName.Admin,
                Description = "Administrator role"
            },
            new RoleEntity
            {
                Id = new Guid("4c5d1784-f350-49da-861c-92c486b4b46c"), // Static Guid
                Name = RoleName.Guest,
                Description = "Guest role"
            }
        );
    }
}