using Microsoft.EntityFrameworkCore;
using DBTransactions.Model;
using System.Reflection;
using DBTransactions.Data;

namespace BankingApi.Data
{
    public class BankingContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public BankingContext(DbContextOptions<BankingContext> options): base(options)
        {
        } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<Account>().HasData(SeedData.SeedAccount());
            modelBuilder.Entity<TransactionLog>().HasData(SeedData.SeedTransactions());
            modelBuilder.Entity<Fee>().HasData(SeedData.SeedFees());
        }
    }
}
