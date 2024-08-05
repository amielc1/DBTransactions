using BankingApi.Data;
using DBTransactions.Model;
using DBTransactions.Transactions;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class TransferController : ControllerBase
{
    private readonly BankingContext _context;
    private readonly AppTransaction _appTransaction;

    public TransferController(BankingContext context, AppTransaction appTransaction)
    {
        _context = context;
        _appTransaction = appTransaction;
    }

    [HttpPost(nameof(TransactionTransfer))]
    public async Task<IActionResult> TransactionTransfer([FromBody] TransferRequest request)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var accountA = _context.Accounts.Single(a => a.Id == request.FromAccountId);
                var accountB = _context.Accounts.Single(b => b.Id == request.ToAccountId);

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



    [HttpPost(nameof(Transfer))]
    public async Task<IActionResult> Transfer([FromBody] TransferRequest request)
    {
        try
        {
            _appTransaction.TransferFunds(request.FromAccountId, request.ToAccountId, request.Amount);
           
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


