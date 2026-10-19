using Microsoft.AspNetCore.Authentication.Cookies;
using Snackis.Models;
using System.Security.Claims;

namespace Snackis.Helpers;

public static class UserPrincipalFactory // Översätter API data från UserViewModel till ASP.
{
    public static ClaimsPrincipal Create(UserViewModel user) //ClaimsPrincipal ASP objekt som representerar identiteten för den inloggade.
    {
        var claims = new List<Claim> // Claim = vilken uppgift som användaren har(id, namn, e-post, roll).
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName)
        };

        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role))); // Gör en claim av varje roll och lägger till dem i listan

        var identity = new ClaimsIdentity(claims,
            CookieAuthenticationDefaults.AuthenticationScheme); // Samlar claims och anger att inloggningen sker med cookies

        return new ClaimsPrincipal(identity);
    }
}
