namespace DBTransactions.Model
{
    public class TransactionLog
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? DeletedSince { get; set; }
        public Status Status { get; set; } = Status.Completed;
    }

}
