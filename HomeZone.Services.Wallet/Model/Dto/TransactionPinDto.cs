using System.ComponentModel.DataAnnotations;

namespace HomeZone.Services.Wallet.Model.Dto
{
    public class TransactionPinDto
    {
        [Required, RegularExpression("\\d{4}")]
        public string TransactionPin { get; set; } = string.Empty;

        [Required, RegularExpression("\\d{4}")]
        public string ConfirmTransactionPin { get; set; } = string.Empty;
    }
}
