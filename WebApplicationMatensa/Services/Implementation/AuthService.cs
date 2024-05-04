using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplicationMatensa.Models.Entity;
using WebApplicationMatensa.Models.RequestModels;
using WebApplicationMatensa.Services.Interface;

namespace WebApplicationMatensa.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthService(UserManager<ApplicationUser> userManager,
               SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string GenerateSecurityToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("MyJwtSceurityMatensaAuthinticateToken");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, user.Email??""),
                    new Claim("UserId", user.Id??""),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<LoginResponseModel> Login(LoginModel loginModel)
        {
            var tmpUser = _userManager.FindByEmailAsync(loginModel.Email).Result;
            if (tmpUser == null)
                return new LoginResponseModel { Success = false };
            var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, true , lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = _userManager.Users.Where(m => m.Email == loginModel.Email).FirstOrDefault();
                var token = GenerateSecurityToken(user);
                return new LoginResponseModel
                {
                    Success = true,
                    Token = token,
                    Id = user.Id
                };
            }
            return new LoginResponseModel { Success = false };
        }

        public async Task<Response> Register(RegisterModel registerModel)
        {
            var user = new ApplicationUser { UserName = registerModel.Email, Email = registerModel.Email,Name = registerModel.Name , BirthDate = registerModel.BirthDate,PhoneNumber = registerModel.PhoneNumber };
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                var currentUser = _userManager.Users.Where(m => m.Email == user.Email).FirstOrDefault();
                //await _signInManager.SignInAsync(user, isPersistent: false);
                var token = GenerateSecurityToken(currentUser);

                return new Response { Success = true };
            }
            else
            {
                return new Response { Success = false};
            }
        }
    }
}
