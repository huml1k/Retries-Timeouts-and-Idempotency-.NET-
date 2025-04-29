using APIGateway.IdempotencyDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIGateway.IdempotencyDb
{
    public class IdempotencyDbContext : DbContext
    {
        public DbSet<HttpDataEntity> httpDataEntities { get; set; }

        public DbSet<IdempotencyKeyEntity> idempotencyKeyEntities { get; set; }

        public IdempotencyDbContext(DbContextOptions<IdempotencyDbContext> optionsBuilder) : base(optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
