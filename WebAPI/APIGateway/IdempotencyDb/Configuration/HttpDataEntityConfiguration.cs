using APIGateway.IdempotencyDb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APIGateway.IdempotencyDb.Configuration
{
    public class HttpDataEntityConfiguration : IEntityTypeConfiguration<HttpDataEntity>
    {
        // HttpDataEntityConfiguration.cs
        public void Configure(EntityTypeBuilder<HttpDataEntity> builder)
        {
            builder.HasKey(x => x.Id);
    
            builder.Property(x => x.RequestDate)
                .HasDefaultValueSql("GETUTCDATE()");
    
            builder.Property(x => x.RequestMethod)
                .IsRequired()
                .HasMaxLength(10); // GET/POST/PUT и т.д.
    
            builder.Property(x => x.RequestPath)
                .IsRequired()
                .HasMaxLength(500);
    
            builder.Property(x => x.ResponseCode)
                .IsRequired();
    
            // Оптимизация для больших данных
            builder.Property(x => x.RequestBody)
                .HasColumnType("nvarchar(max)");
    
            builder.Property(x => x.ResponseBody)
                .HasColumnType("nvarchar(max)");
        }
    }
}
