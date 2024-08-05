namespace DBTransactions.Model
{
    public class Fee
    {
        public int FeeId { get; set; }
        public int TransactionId { get; set; }
        public decimal FeeAmount { get; set; }
        public DateTime FeeDate { get; set; }
        public decimal? PendingFeeAmount { get; set; }
        public DateTime? PendingFeeDate { get; set; }
        public Status Status { get; set; } = Status.None;
    }

}
