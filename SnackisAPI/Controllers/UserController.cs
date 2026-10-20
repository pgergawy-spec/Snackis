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

    [HttpPut("profile-picture")] // PUT /api/users/me/profile-picture: byter bild för den inloggade.
    [RequestSizeLimit(6_000_000)] // Nekar uppladdningar över 6 MB redan innan filen läses.
    public async Task<ActionResult<UserDto>> UploadProfilePicture(IFormFile profilePicture) // Filen kommer som formulärdata (multipart).
        => Ok(await profilePictureService.UploadAsync(CurrentUserId, profilePicture));
}
