using BankingApi.Data;
using DBTransactions.Model;

namespace DBTransactions.Transactions
{
    public class AppTransaction
    {
        private readonly BankingContext context;
        public AppTransaction(BankingContext bankingContext)
        {
            context = bankingContext;
        }


        public void TransferFunds(int sourceAccountId, int targetAccountId, decimal amount)
        {
            try
            {
                var accountA = context.Accounts.Single(a => a.Id == sourceAccountId);
                var accountB = context.Accounts.Single(b => b.Id == targetAccountId);

                if (!(accountA.Status == Status.Completed && accountB.Status == Status.Completed))
                    throw new Exception("The account in Pandeing stauts");

                try
                {
                    accountA.Status = Status.Pending;
                    accountA.Balance = accountA.Balance - amount;

                    accountB.Status = Status.Pending;
                    accountB.Balance = accountB.Balance + amount;

                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    // no roolback - nothing saved 
                    throw;
                }

                try
                {
                    var transactionA = new TransactionLog
                    {
                        AccountId = accountA.Id,
                        Amount = -amount,
                        Status = Status.Pending
                    };

                    var transactionB = new TransactionLog
                    {
                        AccountId = accountB.Id,
                        Amount = amount,
                        Status = Status.Pending
                    };

                    context.TransactionLogs.Add(transactionA);
                    context.TransactionLogs.Add(transactionB);
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    RollbackAccount(context, sourceAccountId, targetAccountId, amount);   
                    throw;
                }

                try
                {
                    var feeA = new Fee
                    {
                        AccountId = accountA.Id,
                        FeeAmount = 2.50m,
                        Status = Status.Pending
                    };

                    context.Fees.Add(feeA);
                    context.SaveChanges();
                }
                catch (Exception)
                {
                   RollbackTransactionLogs(context, sourceAccountId, targetAccountId, amount);
                    throw;
                }

                UpdateStatusToCompleted(context, sourceAccountId, targetAccountId);
            }
            catch
            {
                   RollbackTransactionLogs(context, sourceAccountId, targetAccountId, amount);
                throw;
            }
        }



        private void UpdateStatusToCompleted(BankingContext context, int sourceAccountId, int targetAccountId)
        {
            var accountsToUpdate = context.Accounts
                .Where(a => a.Status == Status.Pending &&
                            (a.Id == sourceAccountId || a.Id == targetAccountId))
                .ToList();

            foreach (var account in accountsToUpdate)
                account.Status = Status.Completed;


            var transactionsToUpdate = context.TransactionLogs
                .Where(t => t.Status == Status.Pending &&
                            (t.AccountId == sourceAccountId || t.AccountId == targetAccountId))
                .ToList();

            foreach (var transaction in transactionsToUpdate)
                transaction.Status = Status.Completed;

            var feesToUpdate = context.Fees
                .Where(f => f.Status == Status.Pending)
                .ToList();

            foreach (var fee in feesToUpdate)
                fee.Status = Status.Completed;

            context.SaveChanges();
        }
         
        private void RollbackAccount(BankingContext context, int sourceAccountId, int targetAccountId, decimal amount)
        {
            try
            {
                var sourceAccount = context.Accounts.Single(x => x.Id == sourceAccountId && x.Status == Status.Pending);
                sourceAccount.Balance += amount;
                sourceAccount.Status = Status.Completed;

                var targetAccount = context.Accounts.Single(x => x.Id == targetAccountId && x.Status == Status.Pending);
                targetAccount.Balance -= amount;
                targetAccount.Status = Status.Completed;

                context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void RollbackTransactionLogs(BankingContext context, int sourceAccountId, int targetAccountId, decimal amount)
        {
            try
            {
                var transactionsToRollback = context.TransactionLogs
                             .Where(t => t.Status == Status.Pending &&
                                         (t.AccountId == sourceAccountId || t.AccountId == targetAccountId))
                             .ToList();

                foreach (var transaction in transactionsToRollback)
                {
                    context.TransactionLogs.Remove(transaction);
                }
                context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
         
        private void RollbackFees(BankingContext context, int sourceAccountId, int targetAccountId, decimal amount)
        {
            try
            {
                var feesToRollback = context.Fees
           .Where(f => f.Status == Status.Pending)
           .ToList();

                foreach (var fee in feesToRollback)
                {
                    context.Fees.Remove(fee);
                }
                context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

        } 





    }
}
