using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snackis.Exceptions;
using Snackis.Models;
using Snackis.Services;

namespace Snackis.Controllers;

[Authorize]
public class CommentController(ICommentApiService commentApiService, ILogger<CommentController> logger) : SnackisControllerBase
{
    [HttpPost("kommentarer/ny")]
    public async Task<IActionResult> Create(CreateCommentViewModel createCommentViewModel)
    {
        if (!ModelState.IsValid)
        {
            ShowError(FirstModelErrorMessage());
            return RedirectToPost(createCommentViewModel.TopicUrlName, createCommentViewModel.PostId, createCommentViewModel.UrlTitle);
        }

        try
        {
            var createdComment = await commentApiService.CreateCommentAsync(createCommentViewModel);
            logger.LogInformation("User {UserId} created comment {CommentId}", CurrentUserId, createdComment.Id);
            ShowSuccess("Din kommentar är publicerad.");
        }
        catch (ApiException apiException) when (IsFormError(apiException))
        {
            ShowError(apiException.Message);
        }

        return RedirectToPost(createCommentViewModel.TopicUrlName, createCommentViewModel.PostId, createCommentViewModel.UrlTitle);
    }

    [HttpPost("kommentarer/{commentId:guid}/radera")]
    public async Task<IActionResult> Delete(Guid commentId, Guid postId, string topicUrlName, string urlTitle)
    {
        await commentApiService.DeleteCommentAsync(commentId);
        logger.LogInformation("User {UserId} deleted comment {CommentId}", CurrentUserId, commentId);

        ShowSuccess("Kommentaren är borttagen.");
        return RedirectToPost(topicUrlName, postId, urlTitle);
    }
}
