using WebApplicationMatensa.Data.BaseRepository;
using WebApplicationMatensa.Data.Repository.Interface;
using WebApplicationMatensa.Models.Entity;

namespace WebApplicationMatensa.Data.Repository.Implementation
{
    public class WalletRepository : RepositoryBase<Wallet>, IWalletRepository
    {
        public WalletRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {

        }
    }
}
