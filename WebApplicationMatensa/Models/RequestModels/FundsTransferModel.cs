using System.ComponentModel.DataAnnotations;

namespace WebApplicationMatensa.Models.RequestModels
{
    public class FundsTransferModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public float Amount { get; set; }
    }
}
