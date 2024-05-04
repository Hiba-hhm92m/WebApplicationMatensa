using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationMatensa.Attributes;
using WebApplicationMatensa.Controllers.User;
using WebApplicationMatensa.Data.Repository.Implementation;
using WebApplicationMatensa.Data.Repository.Interface;
using WebApplicationMatensa.Models.Entity;
using WebApplicationMatensa.Models.RequestModels;
using WebApplicationMatensa.Services.Implementation;
using WebApplicationMatensa.Services.Interface;

namespace WebApplicationMatensa.Controllers.Admin
{
    [AdminAuthorize]
    [ApiController]
    [Route("api/admin/[controller]/[action]")]
    public class UserWalletController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IWalletRepository _WalletRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserWalletController(UserManager<ApplicationUser> userManager, 
            IWalletRepository WalletRepository,
            ILogger<AccountController> logger)
        {
            _logger = logger;
            _WalletRepository = WalletRepository;
            _userManager = userManager;
        }

        [HttpPost(Name = "OpenWallet")]
        public async Task<Response> OpenWallet([FromBody] FundsTransferModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return new Response { Success = false, Message = "User does not exist" };
            }
            var userWallet = await _WalletRepository.SearchFor(x => x.UserId == model.UserId).FirstOrDefaultAsync();
            if (userWallet != null)
            {
                return new Response { Success = false, Message = "User already have a wallet" };
            }
            var wallet = new Wallet
            (
                UserId : model.UserId,
                Balance : model.Amount
            );
            _WalletRepository.Insert(wallet);
            return new Response { Success = true };
        }

        [HttpPut(Name = "AddBalance")]
        public async Task<Response> AddBalance([FromBody] FundsTransferModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return new Response { Success = false, Message = "User does not exist" };
            }
            var userWallet = await _WalletRepository.SearchFor(x => x.UserId == model.UserId).FirstOrDefaultAsync();
            if (userWallet == null)
            {
                return new Response { Success = false, Message = "User does not have wallet" };
            }
            userWallet.AddBalance(model.Amount);
            _WalletRepository.UpdateEntity(userWallet);
            return new Response { Success = true };
        }
    }
}
