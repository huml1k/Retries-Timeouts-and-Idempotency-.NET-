using APIGateway.IdempotencyDb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APIGateway.IdempotencyDb.Configuration
{
    public class IdempotencyKeyConfiguration : IEntityTypeConfiguration<IdempotencyKeyEntity>
    {
        // IdempotencyKeyConfiguration.cs
        public void Configure(EntityTypeBuilder<IdempotencyKeyEntity> builder)
        {
            builder.HasKey(x => x.Id);
    
            builder.Property(x => x.IdempotencyKey)
                .IsRequired()
                .HasMaxLength(256); // Оптимальный размер для GUID/SHA256
    
            builder.HasIndex(x => x.IdempotencyKey)
                .IsUnique()
                .HasDatabaseName("IX_IdempotencyKey_Key");
    
            builder.Property(x => x.CreateDate)
                .HasDefaultValueSql("GETUTCDATE()"); // Автоматическое заполнение
    
            builder.HasOne(x => x.HttpDataEntity)
                .WithOne(x => x.IdempotencyKey)
                .HasForeignKey<IdempotencyKeyEntity>(x => x.HttpExchanceDataID)
                .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление
        }
    }
}
