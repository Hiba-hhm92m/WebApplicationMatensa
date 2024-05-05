using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplicationMatensa.Data.Repository.Interface;
using WebApplicationMatensa.Models.Entity;
using WebApplicationMatensa.Models.RequestModels;
using WebApplicationMatensa.Services.Interface;

namespace WebApplicationMatensa.Controllers.User
{
    [Authorize]
    [ApiController]
    [Route("api/user/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _authService;
        private readonly IWalletRepository _walletRepository;

        public AccountController(UserManager<ApplicationUser> userManager,
            ILogger<AccountController> logger,
            IWalletRepository walletRepository,
            IAuthService authService)
        {
            _logger = logger;
            _userManager = userManager;
            _authService = authService;
            _walletRepository = walletRepository;
        }

        [HttpPost(Name = "Login")]
        [AllowAnonymous]
        public async Task<LoginResponseModel> Login([FromBody] LoginModel model)
        {
            return await _authService.Login(model);
        }

        [HttpPost(Name = "TransferFunds")]
        public async Task<Response> TransferFunds([FromBody] FundsTransferModel model)
        {
            var senderId = getUidFromClaim().ToString();
            var sender = await _userManager.FindByIdAsync(senderId);
            var reciever = await _userManager.FindByIdAsync(model.UserId);
            if(sender==null || reciever == null)
            {
                return new Response { Success = false,Message = "User does not exist" };
            }
            var senderWallet = await _walletRepository.SearchFor(x => x.UserId == sender.Id).FirstOrDefaultAsync();
            var recieverWallet = await _walletRepository.SearchFor(x => x.UserId == reciever.Id).FirstOrDefaultAsync();
            if (senderWallet == null || recieverWallet == null)
            {
                return new Response { Success = false, Message = "User does not have wallet" };
            }
            if(model.Amount> senderWallet.Balance)
            {
                return new Response { Success = false, Message = "User does not have enough money" };
            }
            try
            {
                senderWallet.Withdraw(model.Amount);
                recieverWallet.AddBalance(model.Amount);
                _walletRepository.UpdateEntity(senderWallet);
                _walletRepository.UpdateEntity(recieverWallet);
                return new Response { Success = true};
            }
            catch
            {
                return new Response { Success = false, Message = "error" };
            }
        }

        private Guid? getUidFromClaim()
        {
            JwtSecurityToken tokenS = getToken();
            if (tokenS == null) return null;
            var objTid = tokenS.Payload.Claims.Where(c => c.Type == "UserId").FirstOrDefault();
            if (objTid != null)
            {
                return Guid.Parse(objTid.Value);
            }
            return null;
        }

        private JwtSecurityToken getToken()
        {
            if (!HttpContext.Request.Headers.ContainsKey("authorization")) return null;
            var token = HttpContext.Request.Headers["authorization"].ToString();
            if (token == null) return null;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token.Split(' ')[1]);
            var tokenS = jsonToken as JwtSecurityToken;
            return tokenS;
        }
    }
}
