using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snackis.Exceptions;
using Snackis.Helpers;
using Snackis.Models;
using Snackis.Services;

namespace Snackis.Controllers;

public class AccountController(IAccountApiService accountApiService, ILogger<AccountController> logger) : SnackisControllerBase
{
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(loginViewModel);

        try
        {
            var authResult = await accountApiService.LoginAsync(loginViewModel);
            await SignInAsync(authResult);
            logger.LogInformation("User {UserId} signed in", authResult.User.Id);

            return RedirectToLocal(returnUrl);
        }
        catch (ApiException apiException) when (IsFormError(apiException) || apiException.StatusCode == HttpStatusCode.Unauthorized)
        {
            ModelState.AddModelError(string.Empty, apiException.Message);
            return View(loginViewModel);
        }
    }

    [HttpGet]
    public IActionResult Register()
        => View(new RegisterViewModel());

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid)
            return View(registerViewModel);

        try
        {
            var registeredUser = await accountApiService.RegisterAsync(registerViewModel);
            logger.LogInformation("User {UserId} registered", registeredUser.Id);

            var authResult = await accountApiService.LoginAsync(new LoginViewModel
            {
                UserName = registerViewModel.UserName,
                Password = registerViewModel.Password
            });
            await SignInAsync(authResult);

            ShowSuccess($"Välkommen till Snackis, {registeredUser.UserName}!");
            return RedirectToAction("Index", "Home");
        }
        catch (ApiException apiException) when (IsFormError(apiException))
        {
            ModelState.AddModelError(string.Empty, apiException.Message);
            return View(registerViewModel);
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        logger.LogInformation("User {UserId} signed out", CurrentUserId);

        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Profile() // Visar profilsidan med nuvarande bild.
        => View(await accountApiService.GetCurrentUserAsync());

    [Authorize]
    [HttpPost]
    [RequestSizeLimit(6_000_000)] // Nekar uppladdningar över 6 MB redan innan filen läses.
    public async Task<IActionResult> UploadProfilePicture(IFormFile? profilePicture) // IFormFile = filen som användaren valde i formuläret.
    {
        if (profilePicture is null || profilePicture.Length == 0) // Ingen fil vald eller tom fil.
        {
            ShowError("Välj en bild att ladda upp.");
            return RedirectToAction(nameof(Profile));
        }

        try
        {
            await accountApiService.UploadProfilePictureAsync(profilePicture); // Skickar bilden vidare till API:t, som kontrollerar och sparar den.
            logger.LogInformation("User {UserId} uploaded a new profile picture", CurrentUserId);
            ShowSuccess("Din profilbild är uppdaterad.");
        }
        catch (ApiException apiException) when (IsFormError(apiException))
        {
            ShowError(apiException.Message);
        }

        return RedirectToAction(nameof(Profile));
    }

    [HttpGet]
    public IActionResult AccessDenied()
        => View();

    private async Task SignInAsync(AuthResultViewModel authResult)
    {
        var properties = new AuthenticationProperties
        {
            ExpiresUtc = DateTime.SpecifyKind(authResult.ExpiresAt, DateTimeKind.Utc)
        };
        properties.StoreTokens([new AuthenticationToken { Name = BearerTokenHandler.AccessTokenName, Value = authResult.Token }]);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            UserPrincipalFactory.Create(authResult.User),
            properties);
    }
}
