using Microsoft.EntityFrameworkCore;
using DBTransactions.Model;

namespace BankingApi.Data
{
    public class BankingContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public BankingContext(DbContextOptions<BankingContext> options): base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=sqlserver;Database=BankingDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;");
        }
    }
}
