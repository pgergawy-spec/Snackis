using SnackisAPI.Dtos;

namespace SnackisAPI.Services;

public interface ITokenService
{
    AuthResultDto CreateToken(UserDto user);
}
