using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SnackisAPI.Dtos;
using SnackisAPI.Settings;
using System.Security.Claims;
using System.Text;

namespace SnackisAPI.Services;

public class TokenService(IOptions<JwtSettings> jwtOptions) : ITokenService // IOptions = låda med inställningarna från appsettings, som injiceras (DI)
{
    public AuthResultDto CreateToken(UserDto user)
    {
        var jwtSettings = jwtOptions.Value; // Inställningarna (Issuer, Audience, Key, tid) kommer från appsettings och user secrets.
        var expiresAt = DateTime.UtcNow.AddMinutes(jwtSettings.ExpiresInMinutes); // Token slutar gälla efter ExpiresInMinutes (60 min).

        var claims = new List<Claim> // Claim = uppgift om användaren som sparas i token (id, namn, roll).
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName)
        };
        claims.AddRange(user.Roles.Select(roleName => new Claim(ClaimTypes.Role, roleName))); // Gör en claim av varje roll och lägger till dem i listan.

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)); // Hemlig nyckel som token signeras med, så att ingen kan förfalska den.
        var tokenDescriptor = new SecurityTokenDescriptor // Beskriver hur token ska se ut (claims, giltighet, vem den är till för, signering).
        {
            Subject = new ClaimsIdentity(claims), // Subject = vem token gäller, med alla claims.
            Expires = expiresAt, // När token slutar gälla.
            Issuer = jwtSettings.Issuer, // Issuer = vem som skapade token (API:t).
            Audience = jwtSettings.Audience, // Audience = vem token är till för (klienterna).
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256) // Signerar med nyckeln och algoritmen HS256.
        };

        var token = new JsonWebTokenHandler().CreateToken(tokenDescriptor); // Bygger och signerar token som en textsträng.
        return new AuthResultDto { Token = token, ExpiresAt = expiresAt, User = user }; // Skickar tillbaka token, utgångstid och användaren.
    }
}
