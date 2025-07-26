using HomeZone.Services.SavingsAPI.Model.Dto;
using HomeZone.Services.SavingsAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeZone.Services.SavingsAPI.Controllers
{
    [ApiController]
    [Route("api/savings")]
    public class SavingsPlanController : ControllerBase
    {
        private readonly ISavingsPlanService _service;

        public SavingsPlanController(ISavingsPlanService service)
        {
            _service = service;
        }

        [Authorize]

        [HttpPost("breakdown")]
        public async Task<IActionResult> GetBreakdown([FromBody] SavingsPlanRequestDto dto)
        {
            var result = await _service.CalculateBreakdownAsync(dto);
            return Ok(result);
        }
   
        
        [Authorize]
         [HttpPost]
        public async Task<IActionResult> CreatePlan([FromBody] SavingsPlanRequestDto dto)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var planId = await _service.CreateSavingsPlanAsync(dto, userId);
            return Ok(new { PlanId = planId });
        }
    }

}
