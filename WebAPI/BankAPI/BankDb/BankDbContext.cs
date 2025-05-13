using BankAPI.BankDb.Entities;

namespace BankAPI.BankDb;

using Microsoft.EntityFrameworkCore;

public class BankDbContext : DbContext
{
    public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
    {
    }

    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<AccountEntity> Accounts { get; set; }
    public DbSet<TransactionEntity> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Конфигурация для Customer
        modelBuilder.Entity<CustomerEntity>()
            .HasIndex(c => c.Email)
            .IsUnique();

        modelBuilder.Entity<CustomerEntity>()
            .HasIndex(c => c.PhoneNumber)
            .IsUnique();

        modelBuilder.Entity<CustomerEntity>()
            .HasIndex(c => c.PassportNumber)
            .IsUnique();

        // Конфигурация для Account
        modelBuilder.Entity<AccountEntity>()
            .Property(a => a.AccountNumber)
            .HasMaxLength(20);

        modelBuilder.Entity<AccountEntity>()
            .HasOne(a => a.CustomerEntity)
            .WithMany(c => c.Accounts)
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Конфигурация для Transaction
        modelBuilder.Entity<TransactionEntity>()
            .HasOne(t => t.FromAccountEntity)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.FromAccountNumber)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TransactionEntity>()
            .HasOne(t => t.ToAccountEntity)
            .WithMany()
            .HasForeignKey(t => t.ToAccountNumber)
            .OnDelete(DeleteBehavior.Restrict);
    }
}