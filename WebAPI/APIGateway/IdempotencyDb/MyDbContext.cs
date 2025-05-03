using APIGateway.IdempotencyDb.Configuration;
using APIGateway.IdempotencyDb.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIGateway.IdempotencyDb
{
    public class MyDbContext : DbContext
    {
        public DbSet<HttpDataEntity> httpDataEntities { get; set; }

        public DbSet<IdempotencyKeyEntity> idempotencyKeyEntities { get; set; }

<<<<<<< Updated upstream:WebAPI/APIGateway/IdempotencyDb/MyDbContext.cs
        public MyDbContext(DbContextOptions<MyDbContext> optionsBuilder) : base(optionsBuilder) { }
=======
        public DbSet<UserEntity> userEntities { get; set; }

        public IdempotencyDbContext(DbContextOptions<IdempotencyDbContext> optionsBuilder) : base(optionsBuilder) { }
>>>>>>> Stashed changes:WebAPI/APIGateway/IdempotencyDb/IdempotencyDbContext.cs

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new HttpDataEntityConfiguration());
            modelBuilder.ApplyConfiguration(new IdempotencyKeyConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
