using APIGateway.IdempotencyDb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APIGateway.IdempotencyDb.Configuration
{
    public class HttpDataEntityConfiguration : IEntityTypeConfiguration<HttpDataEntity>
    {
        public void Configure(EntityTypeBuilder<HttpDataEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.IdempotencyKey)
                .WithOne(x => x.HttpDataEntity);
        }
    }
}
