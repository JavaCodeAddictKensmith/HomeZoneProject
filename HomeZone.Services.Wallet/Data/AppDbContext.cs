
using HomeZone.Services.Wallet.Model;

using Microsoft.EntityFrameworkCore;

namespace HomeZone.Services.Wallet.Data
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<WalletDt> Wallets { get; set; }

        //public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
    


}
