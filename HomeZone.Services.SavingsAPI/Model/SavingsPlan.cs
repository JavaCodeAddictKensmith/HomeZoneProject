namespace HomeZone.Services.SavingsAPI.Model
{
    public class SavingsPlan
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } // Link to Identity User
        public string PlanName { get; set; }
        public string Purpose { get; set; }
        public decimal AmountPerPeriod { get; set; }
        public decimal TotalAmount { get; set; }
        public string Frequency { get; set; } // e.g. Weekly, Monthly
        public DateTime StartDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<SavingsSchedule> Schedules { get; set; }
    }

}
