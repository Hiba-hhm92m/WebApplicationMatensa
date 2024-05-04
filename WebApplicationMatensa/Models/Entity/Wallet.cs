using Microsoft.Extensions.Configuration.UserSecrets;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationMatensa.Models.Entity
{
    public class Wallet
    {
        public int Id { get; private set; }
        public string UserId { get; private set; }
        public float Balance { get; private set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public Wallet() { }
        public Wallet(string UserId,float Balance) 
        { 
            this.UserId = UserId;
            this.Balance = Balance;
        }

        public void AddBalance(float Amount)
        {
            this.Balance = this.Balance + Amount;
        }

        public void Withdraw(float Amount)
        {
            this.Balance = this.Balance - Amount;
        }
    }
}
