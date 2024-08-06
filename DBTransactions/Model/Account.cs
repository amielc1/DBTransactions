namespace DBTransactions.Model;

public class Account
{
    public int Id { get; set; }
    public decimal Balance { get; set; }
    public DateTime? DeletedSince { get; set; }
    public Status Status { get; set; } = Status.Completed; 
}
