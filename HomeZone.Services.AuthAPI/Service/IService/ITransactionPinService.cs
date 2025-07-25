namespace HomeZone.Services.AuthAPI.Service.IService
{
    
        public interface ITransactionPinService
        {
            Task<string> SetTransactionPinAsync(string userId, string pin);
            Task<bool> ValidateTransactionPinAsync(string userId, string pin);
        }
    

}
