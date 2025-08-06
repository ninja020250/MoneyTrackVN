using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTrack.Domain.Entities;

namespace MoneyTrack.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<TransactionEntity>
{
    public void Configure(EntityTypeBuilder<TransactionEntity> builder)
    {
        builder.ToTable("transaction");
        builder.HasOne(transaction => transaction.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(trans => trans.UserId);

        builder.Property(trans => trans.CreatedDate)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(transaction => transaction.Category)
            .WithMany(c => c.Transactions)
            .HasForeignKey(transaction => transaction.CategoryId);
    }
}