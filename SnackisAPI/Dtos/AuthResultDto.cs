namespace SnackisAPI.Dtos;

public class AuthResultDto
{
    public string Token { get; set; } = "";
    public DateTime ExpiresAt { get; set; }
    public UserDto User { get; set; } = new();
}
