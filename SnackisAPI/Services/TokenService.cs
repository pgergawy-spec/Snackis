using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SnackisAPI.Dtos;
using SnackisAPI.Settings;

namespace SnackisAPI.Services;

public class TokenService(IOptions<JwtSettings> jwtOptions) : ITokenService
{
    public AuthResultDto CreateToken(UserDto user)
    {
        var jwtSettings = jwtOptions.Value;
        var expiresAt = DateTime.UtcNow.AddMinutes(jwtSettings.ExpiresInMinutes);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName)
        };
        claims.AddRange(user.Roles.Select(roleName => new Claim(ClaimTypes.Role, roleName)));

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt,
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        };

        var token = new JsonWebTokenHandler().CreateToken(tokenDescriptor);
        return new AuthResultDto { Token = token, ExpiresAt = expiresAt, User = user };
    }
}
