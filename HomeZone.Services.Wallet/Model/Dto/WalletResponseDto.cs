namespace HomeZone.Services.Wallet.Model.Dto
{
    public class WalletResponseDto
    {
        public string Id { get; set; } 
        public string Name { get; set; }
        public string WalletNumber { get; set; } // 10-digit unique number
        public decimal Balance { get; set; } = 0;
        public string ApplicationUserId { get; set; }
    }
}
