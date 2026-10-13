using Microsoft.AspNetCore.Mvc;
using SnackisAPI.Dtos;
using SnackisAPI.Services;

namespace SnackisAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        => Ok(await authService.RegisterAsync(registerDto));

    [HttpPost("login")]
    public async Task<ActionResult<AuthResultDto>> Login(LoginDto loginDto)
        => Ok(await authService.LoginAsync(loginDto));
}
