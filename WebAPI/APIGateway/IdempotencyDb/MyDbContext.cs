using APIGateway.IdempotencyDb.Configuration;
using APIGateway.IdempotencyDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIGateway.IdempotencyDb
{
    public class MyDbContext : DbContext
    {
        public DbSet<HttpDataEntity> httpDataEntities { get; set; }
        public DbSet<IdempotencyKeyEntity> idempotencyKeyEntities { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new HttpDataEntityConfiguration());
            modelBuilder.ApplyConfiguration(new IdempotencyKeyConfiguration());
        }
    }
}
