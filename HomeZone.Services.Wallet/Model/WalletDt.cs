//using HomeZone.Services.AuthAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace HomeZone.Services.Wallet.Model
{
    public class WalletDt
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string WalletNumber { get; set; } // 10-digit unique number
        public decimal Balance { get; set; } = 0;
        public string ApplicationUserId { get; set; }
        //public ApplicationUser User { get; set; }
    }
}

//namespace HomeZone.Services.Wallet.Model
//{

//        [Table("Wallets")] // Maps to the "Wallets" table in SQL Server
//        public class Wallet : BaseEntity
//        {
//            [Required]
//            public string UserId { get; set; }

//            [MaxLength(10)]
//            public string Currency { get; set; }

//            // Since SQL Server doesn't support HashSet directly, use a separate table if needed
//            // This can be modeled as a one-to-many or many-to-many if you want normalized lien IDs
//            [NotMapped]
//            public HashSet<string> LienIds { get; set; } = new(); // Needs custom mapping or serialization

//            public decimal TotalInvoiceBalance { get; set; } = 0m;

//            public decimal CurrentBalance { get; set; } = 0m;

//            public decimal AvailableBalance { get; set; } = 0m;

//            public decimal NetIncome { get; set; } = 0m;

//            public decimal NetExpenditure { get; set; } = 0m;

//            [MaxLength(50)]
//            public string Status { get; set; }

//            [MaxLength(100)]
//            public string WalletName { get; set; }

//            // Related virtual accounts, assuming one-to-many relationship (optional)
//            public List<string> VirtualAccountIds { get; set; } = new(); // Not directly storable—needs separate entity

//            public string LienId { get; set; }
//        }

//}
