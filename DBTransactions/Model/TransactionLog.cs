namespace DBTransactions.Model
{
    public class TransactionLog
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal? PendingAmount { get; set; }
        public DateTime? PendingDate { get; set; }
        public Status Status { get; set; } = Status.None;
    }

}
