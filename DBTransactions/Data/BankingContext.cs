using Microsoft.EntityFrameworkCore;
using DBTransactions.Model;

namespace BankingApi.Data
{
    public class BankingContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=BankingDB;User Id=sa;Password=YourStrong@Passw0rd;");
        }
    }
}
