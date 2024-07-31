using BankingApi.Data;
using DBTransactions.Model;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class TransferController : ControllerBase
{
    private readonly BankingContext _context;

    public TransferController(BankingContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> TransactionTransfer([FromBody] TransferRequest request)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var accountA = _context.Accounts.Single(a => a.AccountId == request.FromAccountId);
                var accountB = _context.Accounts.Single(b => b.AccountId == request.ToAccountId);

                if (accountA.Balance < request.Amount)
                {
                    return BadRequest("Insufficient funds.");
                }

                accountA.Balance -= request.Amount;
                accountB.Balance += request.Amount;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<IActionResult> Transfer([FromBody] TransferRequest request)
    {
        try
        {

            var accountA = _context.Accounts.Single(a => a.AccountId == "A");
            var accountB = _context.Accounts.Single(b => b.AccountId == "B");

            if (accountA.PendingUpdate == null && accountB.PendingUpdate == null)
            {
                accountA.PendingBalance = accountA.Balance - 100;
                accountA.PendingUpdate = DateTime.Now;

                accountB.PendingBalance = accountB.Balance + 100;
                accountB.PendingUpdate = DateTime.Now;

                _context.SaveChanges();
            }

            var accountsToUpdate = _context.Accounts
               .Where(a => a.PendingUpdate != null &&
                           (a.AccountId == "A" || a.AccountId == "B"))
               .ToList();

            foreach (var account in accountsToUpdate)
            {
                account.Balance = account.PendingBalance.Value;
                account.LastUpdate = account.PendingUpdate.Value;
                account.PendingBalance = null;
                account.PendingUpdate = null;
            }

            _context.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            // Handle exception and rollback changes if needed
            // Since we are not using a database transaction, ensure atomicity at the application level
            // You may need to implement compensation logic here
            // For example, if updating accountA succeeds but updating accountB fails, revert changes to accountA
            throw new Exception("Transaction failed. Rolling back changes.", ex);
        }
    }
}


