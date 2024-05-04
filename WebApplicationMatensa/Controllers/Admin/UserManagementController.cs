using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationMatensa.Attributes;
using WebApplicationMatensa.Controllers.User;
using WebApplicationMatensa.Models.Dtos;
using WebApplicationMatensa.Models.Entity;
using WebApplicationMatensa.Models.RequestModels;
using WebApplicationMatensa.Services.Interface;

namespace WebApplicationMatensa.Controllers.Admin
{
    [AdminAuthorize]
    [ApiController]
    [Route("api/admin/[controller]/[action]")]
    public class UserManagementController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _authService;

        public UserManagementController(UserManager<ApplicationUser> userManager,
            ILogger<AccountController> logger,
            IAuthService authService)
        {
            _logger = logger;
            _userManager = userManager;
            _authService = authService;
        }

        [HttpPost(Name = "Create")]
        public async Task<Response> Create([FromBody] RegisterModel model)
        {
            return await _authService.Register(model);
        }

        [HttpPut(Name = "Update")]
        public async Task<IdentityResult> Update([FromBody] UpdateUserModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                IdentityResult result = IdentityResult.Failed(new IdentityError
                {
                    Description = "No User Found with this ID"
                });
                return result;
            }
            user.Name = model.Name;
            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            user.BirthDate = model.BirthDate;
            return await _userManager.UpdateAsync(user);
        }

        [HttpDelete(Name = "Delete")]
        public async Task<IdentityResult> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                IdentityResult result = IdentityResult.Failed(new IdentityError
                {
                    Description = "No User Found with this ID"
                });
                return result;
            }
            return await _userManager.DeleteAsync(user);
        }

        [HttpGet(Name = "AllUsers")]
        public async Task<List<ApplicationUserDto>> List()
        {
            var list = await _userManager.Users.Include(x => x.Wallet).ToListAsync();
            return  list.Select(x=> new ApplicationUserDto()
            {
                Id = x.Id,
                UserName = x.UserName,
                Name = x.Name,
                PhoneNumber = x.PhoneNumber,
                BirthDate = x.BirthDate,
                Email = x.Email,
                Balance = x.Wallet!=null ? x.Wallet.Balance : 0
            }).ToList();
        }
    }
}
