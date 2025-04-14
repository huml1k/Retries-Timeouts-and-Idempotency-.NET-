using APIGateway.IdempotencyDb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APIGateway.IdempotencyDb.Configuration
{
    public class IdempotencyKeyConfiguration : IEntityTypeConfiguration<IdempotencyKeyEntity>
    {
        public void Configure(EntityTypeBuilder<IdempotencyKeyEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.HttpDataEntity)
                .WithOne(x => x.IdempotencyKey);
        }
    }
}
