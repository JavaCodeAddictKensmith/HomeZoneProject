namespace HomeZone.Services.SavingsAPI.Model.Dto
{
    public class SavingsBreakdownDto
    {
        public int TotalPayments { get; set; }
        public List<SavingsScheduleDto> Schedule { get; set; }
    }
}
