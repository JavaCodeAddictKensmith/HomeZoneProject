namespace HomeZone.Services.SavingsAPI.Model.Dto
{
    public class SavingsScheduleDto
    {
        public int PaymentNumber { get; set; }
        public DateTime ScheduledDate { get; set; }
        public decimal Amount { get; set; }
    }
}
