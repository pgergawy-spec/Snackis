using SnackisAPI.Dtos;

namespace SnackisAPI.Services;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(RegisterDto registerDto);
    Task<UserDto> LoginAsync(LoginDto loginDto);
    Task<UserDto> GetUserAsync(string userId);
}
