using BankingApi.Data;
using DBTransactions.Model;

namespace DBTransactions.Transactions
{
    public class AppTransaction
    {
        public void TransferFunds(string sourceAccountId, string targetAccountId, decimal amount)
        {
            using (var context = new BankingContext())
            {
                var accountA = context.Accounts.Single(a => a.AccountId == sourceAccountId);
                var accountB = context.Accounts.Single(b => b.AccountId == targetAccountId);

                if (accountA.PendingUpdate == null && accountB.PendingUpdate == null)
                {
                    accountA.PendingBalance = accountA.Balance - amount;
                    accountA.PendingUpdate = DateTime.Now;
                    accountA.Status = Status.Pending;

                    accountB.PendingBalance = accountB.Balance + amount;
                    accountB.PendingUpdate = DateTime.Now;
                    accountB.Status = Status.Pending;

                    context.SaveChanges();

                     var transactionA = new TransactionLog
                    {
                        AccountId = accountA.AccountId,
                        Amount = -amount,
                        PendingAmount = -amount,
                        PendingDate = DateTime.Now,
                        Status = Status.Pending
                    };

                    var transactionB = new TransactionLog
                    {
                        AccountId = accountB.AccountId,
                        Amount = amount,
                        PendingAmount = amount,
                        PendingDate = DateTime.Now,
                        Status = Status.Pending
                    };

                    context.TransactionLogs.Add(transactionA);
                    context.TransactionLogs.Add(transactionB);
                    context.SaveChanges();

                    var feeA = new Fee
                    {
                        TransactionId = transactionA.TransactionId,
                        FeeAmount = 2.50m,
                        PendingFeeAmount = 2.50m,
                        PendingFeeDate = DateTime.Now,
                        Status = Status.Pending
                    };

                    var feeB = new Fee
                    {
                        TransactionId = transactionB.TransactionId,
                        FeeAmount = 2.50m,
                        PendingFeeAmount = 2.50m,
                        PendingFeeDate = DateTime.Now,
                        Status = Status.Pending
                    };

                    context.Fees.Add(feeA);
                    context.Fees.Add(feeB);

                    try
                    {
                        context.SaveChanges();
                        UpdateStatusToCompleted(context, sourceAccountId, targetAccountId);
                    }
                    catch
                    {
                        RollbackPendingChanges(context, sourceAccountId, targetAccountId);
                        throw;
                    }
                }
            }
        }

        private void UpdateStatusToCompleted(BankingContext context, string sourceAccountId, string targetAccountId)
        {
            var accountsToUpdate = context.Accounts
                .Where(a => a.Status == Status.Pending &&
                            (a.AccountId == sourceAccountId || a.AccountId == targetAccountId))
                .ToList();

            foreach (var account in accountsToUpdate)
            {
                account.Balance = account.PendingBalance.Value;
                account.LastUpdate = account.PendingUpdate.Value;
                account.PendingBalance = null;
                account.PendingUpdate = null;
                account.Status = Status.Completed;
            }

            var transactionsToUpdate = context.TransactionLogs
                .Where(t => t.Status == Status.Pending &&
                            (t.AccountId == sourceAccountId || t.AccountId == targetAccountId))
                .ToList();

            foreach (var transaction in transactionsToUpdate)
            {
                transaction.TransactionDate = transaction.PendingDate.Value;
                transaction.PendingAmount = null;
                transaction.PendingDate = null;
                transaction.Status = Status.Completed;
            }

            var feesToUpdate = context.Fees
                .Where(f => f.Status == Status.Pending)
                .ToList();

            foreach (var fee in feesToUpdate)
            {
                fee.FeeDate = fee.PendingFeeDate.Value;
                fee.PendingFeeAmount = null;
                fee.PendingFeeDate = null;
                fee.Status = Status.Completed;
            }

            context.SaveChanges();
        }


        private void RollbackPendingChanges(BankingContext context, string sourceAccountId, string targetAccountId)
        {
            var accountsToRollback = context.Accounts
                .Where(a => a.Status == Status.Pending &&
                            (a.AccountId == sourceAccountId || a.AccountId == targetAccountId))
                .ToList();

            foreach (var account in accountsToRollback)
            {
                if (account.PendingBalance.HasValue)
                {
                    account.Balance = account.Balance - (account.PendingBalance.Value - account.Balance);
                }

                account.PendingBalance = null;
                account.PendingUpdate = null;
                account.Status = Status.None;
            }

            var transactionsToRollback = context.TransactionLogs
                .Where(t => t.Status == Status.Pending &&
                            (t.AccountId == sourceAccountId || t.AccountId == targetAccountId))
                .ToList();

            foreach (var transaction in transactionsToRollback)
            {
                context.TransactionLogs.Remove(transaction);
            }

            var feesToRollback = context.Fees
                .Where(f => f.Status == Status.Pending)
                .ToList();

            foreach (var fee in feesToRollback)
            {
                context.Fees.Remove(fee);
            }

            context.SaveChanges();
        }

    }
}
