using BankAPI.BankDb.Entities;

namespace BankAPI.BankDb;

using Microsoft.EntityFrameworkCore;

public class BankDbContext : DbContext
{
    public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Конфигурация для Customer
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Email)
            .IsUnique();

        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.PhoneNumber)
            .IsUnique();

        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.PassportNumber)
            .IsUnique();

        // Конфигурация для Account
        modelBuilder.Entity<Account>()
            .Property(a => a.AccountNumber)
            .HasMaxLength(20);

        modelBuilder.Entity<Account>()
            .HasOne(a => a.Customer)
            .WithMany(c => c.Accounts)
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Конфигурация для Transaction
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.FromAccount)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.FromAccountNumber)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.ToAccount)
            .WithMany()
            .HasForeignKey(t => t.ToAccountNumber)
            .OnDelete(DeleteBehavior.Restrict);
    }
}