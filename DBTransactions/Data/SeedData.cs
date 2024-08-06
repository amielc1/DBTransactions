using BankingApi.Data;
using DBTransactions.Model;

namespace DBTransactions.Data
{
    public static class SeedData
    {
        public static List<Account> SeedAccount()
        {

            var accounts = new List<Account>
            {
                //new Account
                //{
                //    Id = "A",
                //    Balance = 1000.00m,
                //    Status = Status.None
                //},
                //new Account
                //{
                //    Id = "B",
                //    Balance = 2000.00m,
                //    Status = Status.None
                //},
                //new Account
                //{
                //    Id = "C",
                //    Balance = 1500.00m,
                //    Status = Status.None
                //}
            };

            return accounts;

        }
        public static List<TransactionLog> SeedTransactions()
        {
            var transactions = new List<TransactionLog>
            {
                //new TransactionLog
                //{
                //    Id  = 1,
                //    AccountId = "A",
                //    Amount = -100.00m,
                //    TransactionDate = DateTime.Now.AddDays(-1),
                //    Status = Status.None
                //},
                //new TransactionLog
                //{
                //    Id =2,
                //    AccountId = "B",
                //    Amount = 100.00m,
                //    TransactionDate = DateTime.Now.AddDays(-1),
                //    Status = Status.None
                //},
                //new TransactionLog
                //{
                //    Id = 3 , 
                //    AccountId = "C",
                //    Amount = -50.00m,
                //    TransactionDate = DateTime.Now.AddDays(-2),
                //    Status = Status.None
                //}
            };

            return transactions;
        }
        public static List<Fee> SeedFees()
        {
            var fees = new List<Fee>
            {
                //new Fee
                //{
                //    Id =1 ,
                //    TransactionId = 1,   
                //    FeeAmount = 2.50m,
                //    FeeDate = DateTime.Now.AddDays(-1),
                //    Status = Status.Completed
                //},
                //new Fee
                //{
                //    Id = 2, 
                //    TransactionId = 2,   
                //    FeeAmount = 2.50m,
                //    FeeDate = DateTime.Now.AddDays(-1),
                //    PendingFeeAmount = null,
                //    PendingFeeDate = null,
                //    Status = Status.Completed
                //},
                //new Fee
                //{
                //    Id =3 , 
                //    TransactionId = 3,   
                //    FeeAmount = 1.50m,
                //    FeeDate = DateTime.Now.AddDays(-2),
                //    PendingFeeAmount = null,
                //    PendingFeeDate = null,
                //    Status = Status.Completed
                //}
            };

            return fees;
        }

    }
}
