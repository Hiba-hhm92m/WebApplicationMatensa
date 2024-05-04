using Microsoft.AspNetCore.Identity;

namespace WebApplicationMatensa.Models.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public virtual Wallet Wallet { get; set; }
    }
}
