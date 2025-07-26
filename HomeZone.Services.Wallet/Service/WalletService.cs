
using HomeZone.Services.Wallet.Data;
using HomeZone.Services.Wallet.Model;
using HomeZone.Services.Wallet.Service.IService;
using HomeZone.Services.Wallet.Wrappers;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using HomeZone.Services.Wallet.Model.Dto;
using Org.BouncyCastle.Security;

namespace HomeZone.Services.Wallet.Service
{
    public class WalletService : IWalletService
    {



        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITransactionPinService _transactionPinService;
        decimal withdrawalLimit = 0;


        public WalletService(
            IHttpContextAccessor httpContextAccessor,
            AppDbContext db,
            ITransactionPinService transactionPinService)
        {
            _httpContextAccessor = httpContextAccessor;
            _db = db;
            _transactionPinService = transactionPinService;
        }

        public async Task<ResponseWrapper<string>> CreateWalletAsyc(string name)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return new ResponseWrapper<string>(null, "User not authenticated.", false);

            if (_db.Wallets.Any(w => w.ApplicationUserId == userId))
                return new ResponseWrapper<string>(null, "Wallet already exists.", false);

            string walletNumber;
            do
            {
                walletNumber = new Random().Next(1000000000, int.MaxValue).ToString();
            } while (_db.Wallets.Any(w => w.WalletNumber == walletNumber));

            var wallet = new WalletDt
            {
                Name = name,
                WalletNumber = walletNumber,
                ApplicationUserId = userId
            };

            _db.Wallets.Add(wallet);
            await _db.SaveChangesAsync();

            return new ResponseWrapper<string>(wallet.WalletNumber, "Wallet created successfully.", true);
        }

        public async Task<ResponseWrapper<string>> FundWalletAsyc(decimal amount, string pin)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return new ResponseWrapper<string>(null, "User not authenticated.", false);

            if (amount <= 150)
                return new ResponseWrapper<string>(null, "Amount must be greater than 0.", false);

            var isValid = await _transactionPinService.ValidateTransactionPinAsync(userId, pin);
            if (!isValid)
                return new ResponseWrapper<string>(null, "Invalid transaction PIN.", false);

            var wallet = _db.Wallets.FirstOrDefault(w => w.ApplicationUserId == userId);
            if (wallet == null)
                return new ResponseWrapper<string>(null, "Wallet not found.", false);

            wallet.Balance += amount;
            _db.Wallets.Update(wallet);
            await _db.SaveChangesAsync();

            return new ResponseWrapper<string>(null, "Wallet funded successfully.", true);
        }

        public async Task<ResponseWrapper<WalletResponseDto>> GetWalletDetailsAsync(string WalletId)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return new ResponseWrapper<WalletResponseDto>(null, "User not authenticated.", false);

            if (!Guid.TryParse(WalletId, out Guid walletGuid))
                return new ResponseWrapper<WalletResponseDto>(null, "Invalid Wallet ID format.", false);

            var wallet = await _db.Wallets.FirstOrDefaultAsync(w => w.Id == walletGuid && w.ApplicationUserId == userId);

            if (wallet == null)
                return new ResponseWrapper<WalletResponseDto>(null, "Wallet not found.", false);

            var walletResult = new WalletResponseDto
            {
                Id = wallet.Id.ToString(),
                Name = wallet.Name,
                WalletNumber = wallet.WalletNumber,
                Balance = wallet.Balance,
                ApplicationUserId = wallet.ApplicationUserId
            };

            return new ResponseWrapper<WalletResponseDto>(walletResult, "Wallet fetched successfully.", true);
        }





        public async Task<ResponseWrapper<string>> TransferFromWalletToAnotherWalletAsync(string WalletAccount, decimal Amount, string pin)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return new ResponseWrapper<string>(null, "You are not authenticated.", false);

            var mywallet = await _db.Wallets.FirstOrDefaultAsync(w => w.ApplicationUserId == userId);
            if (mywallet == null)
                return new ResponseWrapper<string>(null, "Your Wallet was not found.", false);

            var isValid = await _transactionPinService.ValidateTransactionPinAsync(userId, pin);
            if (!isValid)
                return new ResponseWrapper<string>(null, "Invalid transaction PIN.", false);

            if (Amount <= 0)
                return new ResponseWrapper<string>(null, "Amount must be greater than 0.", false);

            if (mywallet.Balance <= 100)
                return new ResponseWrapper<string>(null, "Your wallet balance must be greater than 100 before transferring.", false);

            decimal withdrawalLimit = mywallet.Balance - 100;
            if (Amount > withdrawalLimit)
                return new ResponseWrapper<string>(null, $"Insufficient available balance. Maximum transferable amount is {withdrawalLimit}.", false);

            var receipientWallet = await _db.Wallets.FirstOrDefaultAsync(w => w.WalletNumber == WalletAccount);
            if (receipientWallet == null)
                return new ResponseWrapper<string>(null, "Recipient wallet not found.", false);
            if (receipientWallet.WalletNumber == mywallet.WalletNumber)
                return new ResponseWrapper<string>(null, "You can only transfer to another account.", false);

            // Perform transfer
            mywallet.Balance -= Amount;
            receipientWallet.Balance += Amount;

            _db.Wallets.Update(mywallet);
            _db.Wallets.Update(receipientWallet);
            await _db.SaveChangesAsync();

            return new ResponseWrapper<string>(
                null,
                $"Transferred {Amount} to {receipientWallet.Name} (Wallet No: {receipientWallet.WalletNumber}) successfully. Your new balance is {mywallet.Balance}.",
                true
            );
        }


        public async Task<ResponseWrapper<string>> WithDrawFromWalletAsync( decimal Amount, string pin)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return new ResponseWrapper<string>(null, "User not authenticated.", false);
            var wallet = _db.Wallets.FirstOrDefault(w => w.ApplicationUserId == userId);
            //decimal withdrawalLimit = 0;
           
            //if (amount <= 0)
            //    return new ResponseWrapper<string>(null, "Amount must be greater than 0.", false);

            var isValid = await _transactionPinService.ValidateTransactionPinAsync(userId, pin);
            if (!isValid)
                return new ResponseWrapper<string>(null, "Invalid transaction PIN.", false);



            if (wallet.Balance > 100)
            {
                withdrawalLimit = wallet.Balance - 100.00m;
            }
            else
            {
                return new ResponseWrapper<string>(null, "Your Wallet balance must be greater than 100 before you can.", false);
            }



            if (wallet == null)
                return new ResponseWrapper<string>(null, "Wallet not found.", false);

            if(wallet.Balance>100 && Amount <= withdrawalLimit)
            {
                wallet.Balance -= Amount;
                _db.Wallets.Update(wallet);
                await _db.SaveChangesAsync();

                return new ResponseWrapper<string>(null, $"WithDrawn {Amount} from the Wallet Successfully.", true);
            }
            return new ResponseWrapper<string>(null, $"Error occured", false);


        }
    }
}
