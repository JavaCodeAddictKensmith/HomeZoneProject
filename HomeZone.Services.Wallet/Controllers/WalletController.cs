
using HomeZone.Services.Wallet.Model;
using HomeZone.Services.Wallet.Service.IService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeZone.Services.Wallet.Controllers
{
    //[Route("api/walletapi")]
    [ApiController]
   
    [Route("api/walletapi")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPost("create-wallet")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> CreateWallet([FromQuery] string name)
        {
            var result = await _walletService.CreateWalletAsyc(name);
            return result.Success ? Ok(result) : BadRequest(result);
        }

       
       

    
        [HttpPost("fund-wallet")]
        
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> FundWallet([FromBody] FundWalletRequest request)
        {
            var result = await _walletService.FundWalletAsyc(request.Amount, request.Pin);
            return result.Success ? Ok(result) : BadRequest(result);
        }


    //    [HttpGet("view-wallet-details/{walletId}")]

       
    //    public async Task<IActionResult> GetWalletDetails(string walletId)
    //    {
    //        var result = await _walletService.GetWalletDetailsAsync(walletId);
    //        return result.Success ? Ok(result) : BadRequest(result);
    //          //public string WalletId { get; set; } = string.Empty;
    //}

        [HttpGet("view-wallet-details/{walletId}")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> GetWalletDetailsAsync(string walletId)
        {
            var result = await _walletService.GetWalletDetailsAsync(walletId);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }


        [HttpPost("withdraw-from-wallet")]

        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> WithdrawFromWallet([FromBody] FundWalletRequest request)
        {
            var result = await _walletService.WithDrawFromWalletAsync(request.Amount, request.Pin);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("transfer-fund-from-wallet")]

        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> TransferFromWalletToWallet([FromBody] TranferFromWalletRequest request)
        {
            var result = await _walletService.TransferFromWalletToAnotherWalletAsync(request.walletNumber,request.Amount, request.Pin);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        

    }
}
