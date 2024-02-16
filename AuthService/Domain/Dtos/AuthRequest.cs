namespace AuthService.Domain.Dtos;

public class AuthRequest
{
    public string Login { get; set; }
    public string Password { get; set; }
}