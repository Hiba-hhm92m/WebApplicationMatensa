using System.ComponentModel.DataAnnotations;

namespace WebApplicationMatensa.Models.RequestModels
{
    public class FundsTransferModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public float Amount { get; set; }
    }
}
