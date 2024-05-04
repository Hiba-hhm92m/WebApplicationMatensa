using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationMatensa.Data;
using WebApplicationMatensa.Data.Repository.Implementation;
using WebApplicationMatensa.Data.Repository.Interface;

namespace WebApplicationMatensa.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ApplicationDbContext _aplicationDbContext;
        private IWalletRepository _wallet;

        public RepositoryWrapper(ApplicationDbContext aplicationDbContext)
        {
            _aplicationDbContext = aplicationDbContext;
        }

        public IWalletRepository Wallet
        {
            get
            {
                if (_wallet == null)
                {
                    _wallet = new WalletRepository(_aplicationDbContext);
                }
                return _wallet;
            }
        }
    }
}
