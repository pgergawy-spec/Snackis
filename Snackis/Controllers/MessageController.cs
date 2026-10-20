using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snackis.Exceptions;
using Snackis.Models;
using Snackis.Services;

namespace Snackis.Controllers;

[Authorize]
public class MessageController(IMessageApiService messageApiService, ILogger<MessageController> logger) : SnackisControllerBase
{
    public async Task<IActionResult> Inbox()
        => View("List", new MessageListViewModel
        {
            Heading = "Inkorg",
            ShowSender = true,
            Messages = await messageApiService.GetInboxAsync()
        });

    public async Task<IActionResult> Sent()
        => View("List", new MessageListViewModel
        {
            Heading = "Skickade meddelanden",
            ShowSender = false,
            Messages = await messageApiService.GetSentAsync()
        });

    [HttpGet]
    public IActionResult Create(string? to)
        => View(new SendMessageViewModel { ReceiverUserName = to ?? "" });

    [HttpPost]
    public async Task<IActionResult> Create(SendMessageViewModel sendMessageViewModel)
    {
        if (!ModelState.IsValid)
            return View(sendMessageViewModel);

        try
        {
            var sentMessage = await messageApiService.SendMessageAsync(sendMessageViewModel);
            logger.LogInformation("User {UserId} sent message {MessageId}", CurrentUserId, sentMessage.Id);

            ShowSuccess($"Meddelandet är skickat till {sentMessage.ReceiverName}.");
            return RedirectToAction(nameof(Sent));
        }
        catch (ApiException apiException) when (IsFormError(apiException))
        {
            ModelState.AddModelError(string.Empty, apiException.Message);
            return View(sendMessageViewModel);
        }
    }
}
