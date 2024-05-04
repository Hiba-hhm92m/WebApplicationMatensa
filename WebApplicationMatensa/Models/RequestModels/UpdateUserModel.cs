using System.ComponentModel.DataAnnotations;

namespace WebApplicationMatensa.Models.RequestModels
{
    public class UpdateUserModel
    {
        [Required]
        public string Id { set; get; }
        public string Name { set; get; }
        public string UserName { set; get; }
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { set; get; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
    }
}
