using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Snackis.Exceptions;

namespace Snackis.Controllers;

public abstract class SnackisControllerBase : Controller
{
    protected string? CurrentUserId
        => User.FindFirstValue(ClaimTypes.NameIdentifier);

    protected bool IsCurrentUserAdmin
        => User.IsInRole("Admin");

    protected static bool IsFormError(ApiException apiException)
        => apiException.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.Conflict or HttpStatusCode.NotFound;

    protected void ShowSuccess(string message)
        => TempData["SuccessMessage"] = message;

    protected void ShowError(string message)
        => TempData["ErrorMessage"] = message;

    protected string FirstModelErrorMessage()
        => ModelState.Values
            .SelectMany(entry => entry.Errors)
            .Select(error => error.ErrorMessage)
            .FirstOrDefault() ?? "Uppgifterna är inte giltiga.";

    protected IActionResult RedirectToPost(string topicUrlName, Guid postId, string urlTitle)
        => RedirectToAction("Details", "Post", new { topicUrlName, postId, urlTitle });

    protected IActionResult RedirectToLocal(string? returnUrl)
        => !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? LocalRedirect(returnUrl)
            : RedirectToAction("Index", "Home");
}
