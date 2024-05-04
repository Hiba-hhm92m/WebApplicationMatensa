using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplicationMatensa.Models.Entity;

namespace WebApplicationMatensa.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Wallet> Wallets { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Wallet>().HasOne(x => x.User)
                .WithOne(y => y.Wallet)
                .HasForeignKey<Wallet>(x=>x.UserId);
        }
    }
}
