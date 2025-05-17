using APIGateway.IdempotencyDb.Configuration;
using APIGateway.IdempotencyDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIGateway.IdempotencyDb
{
    public class IdempotencyDbContext : DbContext
    {
        public DbSet<HttpDataEntity> httpDataEntities { get; set; }

        public DbSet<IdempotencyKeyEntity> idempotencyKeyEntities { get; set; }

        public DbSet<UserEntity> userEntities { get; set; }

        public DbSet<FinancialProfile> financialProfiles { get; set; }

        public IdempotencyDbContext(DbContextOptions<IdempotencyDbContext> optionsBuilder) : base(optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FinancialProfileConfiguration());
            modelBuilder.ApplyConfiguration(new HttpDataEntityConfiguration());
            modelBuilder.ApplyConfiguration(new IdempotencyKeyConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
