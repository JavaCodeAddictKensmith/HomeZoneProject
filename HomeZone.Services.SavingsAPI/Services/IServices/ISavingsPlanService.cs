using HomeZone.Services.SavingsAPI.Model.Dto;

namespace HomeZone.Services.SavingsAPI.Services.IServices
{
    public interface ISavingsPlanService
    {
        Task<SavingsBreakdownDto> CalculateBreakdownAsync(SavingsPlanRequestDto dto);
        Task<Guid> CreateSavingsPlanAsync(SavingsPlanRequestDto dto, string userId);
    }

}
