namespace AuthService.Domain.Dtos
{
    public class RegisterRequest
    {
        public string login { get; set; }
        public string password { get; set; }
        public string avatar { get; set; }
    }
}
