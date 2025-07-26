namespace HomeZone.Services.SavingsAPI.Model
{
    public class SavingsSchedule
    {
        public Guid Id { get; set; }
        public Guid SavingsPlanId { get; set; }
        public SavingsPlan SavingsPlan { get; set; }

        public int PaymentNumber { get; set; }
        public DateTime ScheduledDate { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; } = false;
    }

}
