using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Snackis.Models;
using Snackis.Services;

namespace Snackis.Controllers;

public class HomeController(ITopicApiService topicApiService) : SnackisControllerBase
{
    public async Task<IActionResult> Index()
        => View(await topicApiService.GetAllTopicsAsync());

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int? statusCode)
    {
        var message = statusCode switch
        {
            404 => "Sidan du letar efter finns inte.",
            403 => "Du har inte behörighet att se den här sidan.",
            _ => "Något gick fel. Försök igen om en stund."
        };

        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            Message = message
        });
    }
}
