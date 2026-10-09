using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnackisAPI.Dtos;
using SnackisAPI.Services;

namespace SnackisAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/messages")]
public class PrivateMessageController(IPrivateMessageService messageService) : SnackisControllerBase
{
    [HttpGet("inbox")]
    public async Task<ActionResult<IEnumerable<PrivateMessageDto>>> GetInbox()
        => Ok(await messageService.GetInboxAsync(CurrentUserId));

    [HttpGet("sent")]
    public async Task<ActionResult<IEnumerable<PrivateMessageDto>>> GetSent()
        => Ok(await messageService.GetSentAsync(CurrentUserId));

    [HttpPost]
    public async Task<ActionResult<PrivateMessageDto>> Send(SendMessageDto sendMessageDto)
        => Ok(await messageService.SendAsync(sendMessageDto, CurrentUserId));
}
