namespace DBTransactions.Model
{
    public class Fee
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal FeeAmount { get; set; }
        public DateTime FeeDate { get; set; }
        public DateTime? DeletedSince { get; set; }
        public Status Status { get; set; } = Status.Completed;
    }

}
