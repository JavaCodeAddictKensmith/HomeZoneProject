namespace HomeZone.Services.Wallet.Service.IService
{
    public interface ITransactionPinService
    {
        Task<bool> ValidateTransactionPinAsync(string userId, string pin);
    }
}
