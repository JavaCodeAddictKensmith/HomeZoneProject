using HomeZone.Services.Wallet.Service.IService;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace HomeZone.Services.Wallet.Service
{
    public class TransactionPinService : ITransactionPinService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TransactionPinService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> ValidateTransactionPinAsync(string userId, string pin)
        {
            var client = _httpClientFactory.CreateClient("Auth");

            var response = await client.PostAsJsonAsync($"/api/transactionpin/{userId}/validate-pin", new { Pin = pin });

            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<ValidationResponse>();

            return result?.IsValid ?? false;
        }

        private class ValidationResponse
        {
            public bool IsValid { get; set; }
        }
    }
}
