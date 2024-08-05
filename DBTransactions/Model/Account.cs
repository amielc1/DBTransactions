namespace DBTransactions.Model;

public class Account
{
    public string Id { get; set; }
    public decimal Balance { get; set; }
    public DateTime LastUpdate { get; set; }
    public decimal? PendingBalance { get; set; }
    public DateTime? PendingUpdate { get; set; }
    public Status Status { get; set; } = Status.None; 
}
