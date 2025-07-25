using HomeZone.Services.Wallet.Model.Dto;
using HomeZone.Services.Wallet.Wrappers;

namespace HomeZone.Services.Wallet.Service.IService
{
    public interface IWalletService
    {
        Task<ResponseWrapper<string>> CreateWalletAsyc( string name);
        Task<ResponseWrapper<string>> FundWalletAsyc( decimal amount, string pin);
        Task<ResponseWrapper<string>> WithDrawFromWalletAsync( decimal Amount, string pin);
        Task<ResponseWrapper<string>> TransferFromWalletToAnotherWalletAsync(string WalletAccount, decimal Amount, string pin);
        Task<ResponseWrapper<WalletResponseDto>>GetWalletDetailsAsync(string WalletId);

    }
}
