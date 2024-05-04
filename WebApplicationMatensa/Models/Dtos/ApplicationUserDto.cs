namespace WebApplicationMatensa.Models.Dtos
{
    public class ApplicationUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public float Balance { get; set; }
    }
}
