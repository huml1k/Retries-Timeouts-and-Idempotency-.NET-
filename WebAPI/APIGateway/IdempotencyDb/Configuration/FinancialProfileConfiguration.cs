using APIGateway.IdempotencyDb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Reflection.Emit;

namespace APIGateway.IdempotencyDb.Configuration
{
    public class FinancialProfileConfiguration : IEntityTypeConfiguration<FinancialProfile>
    {
        public void Configure(EntityTypeBuilder<FinancialProfile> modelBuilder)
        {
            modelBuilder
                .HasOne(fp => fp.User)          
                .WithOne(u => u.FinancialProfile) 
                .HasForeignKey<FinancialProfile>(fp => fp.UserId) 
                .OnDelete(DeleteBehavior.Cascade); 

            
            modelBuilder.Property(fp => fp.AccountNumber)
                .IsRequired()
                .HasMaxLength(34);

            modelBuilder.Property(fp => fp.Balance)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Property(fp => fp.UnpaidCredit)
                .HasColumnType("decimal(18,2)");
        }
    }
}
