namespace Snackis.Models;

public class AvatarViewModel
{
    public string? PictureUrl { get; set; }
    public string UserName { get; set; } = "";
    public string Size { get; set; } = "md";

    public string Initial => UserName.Length > 0 ? UserName[..1].ToUpperInvariant() : "?";
}
