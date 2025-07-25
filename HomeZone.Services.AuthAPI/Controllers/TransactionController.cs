using HomeZone.Services.AuthAPI.Models.Dto;
using HomeZone.Services.AuthAPI.Service.IService;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeZone.Services.AuthAPI.Controllers
{
    
    [Route("api/transactionpin")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionPinService _transactionPinService;

        public TransactionController(ITransactionPinService transactionPinService)
        {
            _transactionPinService = transactionPinService;
        }

        [HttpPost("{userId}/set-pin")]
        public async Task<IActionResult> SetPin(string userId, [FromBody] SetPinDto dto)
        {
            var result = await _transactionPinService.SetTransactionPinAsync(userId, dto.Pin);
            return string.IsNullOrEmpty(result)
                ? Ok(new { Message = "PIN set successfully." })
                : BadRequest(new { Message = result });
        }

        [HttpPost("{userId}/validate-pin")]
        public async Task<IActionResult> ValidatePin(string userId, [FromBody] ValidatePinDto dto)
        {
            var isValid = await _transactionPinService.ValidateTransactionPinAsync(userId, dto.Pin);
            return Ok(new { IsValid = isValid });
        }

       
    }

}
