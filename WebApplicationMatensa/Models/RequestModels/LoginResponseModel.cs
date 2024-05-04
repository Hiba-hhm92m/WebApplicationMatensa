namespace WebApplicationMatensa.Models.RequestModels
{
    public class LoginResponseModel
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Id { get; set; }
    }
}
