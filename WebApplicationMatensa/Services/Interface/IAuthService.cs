using WebApplicationMatensa.Models.Entity;
using WebApplicationMatensa.Models.RequestModels;

namespace WebApplicationMatensa.Services.Interface
{
    public interface IAuthService
    {
        public string GenerateSecurityToken(ApplicationUser user);
        public Task<LoginResponseModel> Login(LoginModel loginModel);
        public Task<Response> Register(RegisterModel registerModel);
    }
}
