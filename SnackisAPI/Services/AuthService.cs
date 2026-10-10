using Microsoft.AspNetCore.Identity;
using SnackisAPI.Dtos;
using SnackisAPI.Exceptions;
using SnackisAPI.Mappings;
using SnackisAPI.Models;

namespace SnackisAPI.Services;

public class AuthService(UserManager<User> userManager, ILogger<AuthService> logger) : IAuthService
{
    private const string DefaultRoleName = "User";

    public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
    {
        var user = new User
        {
            UserName = registerDto.UserName.Trim(),
            Email = registerDto.Email.Trim()
        };

        var creationResult = await userManager.CreateAsync(user, registerDto.Password);
        if (!creationResult.Succeeded)
            throw CreateRegistrationException(creationResult);

        await userManager.AddToRoleAsync(user, DefaultRoleName);
        logger.LogInformation("User {UserId} registered with user name {UserName}", user.Id, user.UserName);

        return await ToUserDtoAsync(user);
    }

    public async Task<UserDto> LoginAsync(LoginDto loginDto)
    {
        var user = await userManager.FindByNameAsync(loginDto.UserName);
        var isPasswordCorrect = user is not null && await userManager.CheckPasswordAsync(user, loginDto.Password);

        if (user is null || !isPasswordCorrect)
        {
            logger.LogWarning("Failed login attempt for user name {UserName}", loginDto.UserName);
            throw new UnauthorizedException("Fel användarnamn eller lösenord.");
        }

        return await ToUserDtoAsync(user);
    }

    public async Task<UserDto> GetUserAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("Användaren finns inte.");

        return await ToUserDtoAsync(user);
    }

    private async Task<UserDto> ToUserDtoAsync(User user)
        => user.ToDto(await userManager.GetRolesAsync(user));

    private static ApiException CreateRegistrationException(IdentityResult creationResult)
    {
        var errorCodes = creationResult.Errors.Select(error => error.Code).ToList();

        if (errorCodes.Contains("DuplicateUserName"))
            return new ConflictException("Användarnamnet är upptaget.");

        if (errorCodes.Contains("DuplicateEmail"))
            return new ConflictException("E-postadressen används redan.");

        if (errorCodes.Any(errorCode => errorCode.StartsWith("Password")))
            return new BadRequestException("Lösenordet måste ha minst 8 tecken med stor och liten bokstav, siffra och specialtecken.");

        return new BadRequestException(string.Join(" ", creationResult.Errors.Select(error => error.Description)));
    }
}
