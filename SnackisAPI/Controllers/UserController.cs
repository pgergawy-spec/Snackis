using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnackisAPI.Dtos;
using SnackisAPI.Services;

namespace SnackisAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/users/me")]
public class UserController(IAuthService authService, IProfilePictureService profilePictureService) : SnackisControllerBase
{
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
        => Ok(await authService.GetUserAsync(CurrentUserId));

    [HttpPut("profile-picture")]
    [RequestSizeLimit(6_000_000)]
    public async Task<ActionResult<UserDto>> UploadProfilePicture(IFormFile profilePicture)
        => Ok(await profilePictureService.UploadAsync(CurrentUserId, profilePicture));
}
