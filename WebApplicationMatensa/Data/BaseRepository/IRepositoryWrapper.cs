using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationMatensa.Data.Repository.Interface;

namespace WebApplicationMatensa.Repositories
{
    public interface IRepositoryWrapper
    {
        IWalletRepository Wallet { get; }
    }
}
