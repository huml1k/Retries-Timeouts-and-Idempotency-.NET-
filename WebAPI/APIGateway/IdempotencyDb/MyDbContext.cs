using APIGateway.IdempotencyDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIGateway.IdempotencyDb
{
    public class MyDbContext : DbContext
    {
        public DbSet<HttpDataEntity> httpDataEntities { get; set; }

        public DbSet<IdempotencyKeyEntity> idempotencyKeyEntities { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> optionsBuilder) : base(optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
