namespace Snackis.Models;

public class AuthResultViewModel //Tar emot svaret för inloggning
{
    public string Token { get; set; } = "";
    public DateTime ExpiresAt { get; set; }
    public UserViewModel? User { get; set; }
}
