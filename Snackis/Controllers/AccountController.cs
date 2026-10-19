using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Snackis.Helpers;
using Snackis.Models;
using Snackis.Services;

namespace Snackis.Controllers;

public class AccountController(IAccountApiService accountApiService) : SnackisControllerBase
{
    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]

    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(loginViewModel);
        }

        var authResult = await accountApiService.LoginAsync(loginViewModel);
        var properties = new AuthenticationProperties
        {
            ExpiresUtc = authResult.ExpiresAt
        };
        properties.StoreTokens([new AuthenticationToken { Name = "access_token", Value = authResult.Token }]);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            UserPrincipalFactory.Create(authResult.User!), properties);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}