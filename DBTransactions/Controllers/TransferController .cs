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
    public async Task<IActionResult> Transfer([FromBody] TransferRequest request)
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
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}


