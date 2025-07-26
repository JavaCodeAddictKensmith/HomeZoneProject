namespace HomeZone.Services.SavingsAPI.Model.Dto
{
    public class SavingsPlanRequestDto
    {
        public string PlanName { get; set; }
        public string Purpose { get; set; }
        public decimal AmountPerPeriod { get; set; }
        public decimal TotalAmount { get; set; }
        public string Frequency { get; set; } // Weekly or Monthly
        public DateTime StartDate { get; set; }
    }

}
