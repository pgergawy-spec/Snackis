using Microsoft.AspNetCore.Identity;

namespace SnackisAPI.Models;

public class User : IdentityUser
{
    public string? ProfilePictureUrl { get; set; }
}
