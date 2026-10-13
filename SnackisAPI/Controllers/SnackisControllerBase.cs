using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SnackisAPI.Exceptions;

namespace SnackisAPI.Controllers;

public abstract class SnackisControllerBase : ControllerBase
{
    protected string CurrentUserId
        => User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedException("Du måste vara inloggad.");

    protected bool IsCurrentUserAdmin
        => User.IsInRole("Admin");
}
