namespace HomeZone.Services.Wallet.Model
{
    public class TranferFromWalletRequest
    {  public string walletNumber {  get; set; }
       public decimal Amount { get; set; }
        public string Pin { get; set; }
    }
}
