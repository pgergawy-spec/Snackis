using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snackis.Exceptions;
using Snackis.Models;
using Snackis.Services;

namespace Snackis.Controllers;

public class PostController(
    IPostApiService postApiService,
    ICommentApiService commentApiService,
    ITopicApiService topicApiService,
    ILogger<PostController> logger) : SnackisControllerBase
{
    [HttpGet("forum/{topicUrlName}/{postId:guid}/{urlTitle?}")]
    public async Task<IActionResult> Details(Guid postId)
    {
        var post = await postApiService.GetPostAsync(postId);

        return View(new PostPageViewModel
        {
            Post = post,
            Comments = await commentApiService.GetCommentsAsync(postId),
            NewComment = new CreateCommentViewModel
            {
                PostId = post.Id,
                TopicUrlName = post.TopicUrlName,
                UrlTitle = post.UrlTitle
            },
            CurrentUserId = CurrentUserId,
            IsAdmin = IsCurrentUserAdmin
        });
    }

    [Authorize]
    [HttpGet("forum/{topicUrlName}/nytt-inlagg")]
    public async Task<IActionResult> Create(string topicUrlName)
    {
        var topic = await topicApiService.GetTopicAsync(topicUrlName);

        return View(new CreatePostViewModel
        {
            TopicId = topic.Id,
            TopicName = topic.Name,
            TopicUrlName = topic.UrlName
        });
    }

    [Authorize]
    [HttpPost("forum/{topicUrlName}/nytt-inlagg")]
    public async Task<IActionResult> Create(CreatePostViewModel createPostViewModel)
    {
        if (!ModelState.IsValid)
            return View(createPostViewModel);

        try
        {
            var createdPost = await postApiService.CreatePostAsync(createPostViewModel);
            logger.LogInformation("User {UserId} created post {PostId} in topic {TopicUrlName}", CurrentUserId, createdPost.Id, createPostViewModel.TopicUrlName);

            ShowSuccess("Ditt inlägg är publicerat.");
            return RedirectToPost(createPostViewModel.TopicUrlName, createdPost.Id, createdPost.UrlTitle);
        }
        catch (ApiException apiException) when (IsFormError(apiException))
        {
            ModelState.AddModelError(string.Empty, apiException.Message);
            return View(createPostViewModel);
        }
    }

    [Authorize]
    [HttpPost("forum/{topicUrlName}/{postId:guid}/radera")]
    public async Task<IActionResult> Delete(string topicUrlName, Guid postId)
    {
        await postApiService.DeletePostAsync(postId);
        logger.LogInformation("User {UserId} deleted post {PostId}", CurrentUserId, postId);

        ShowSuccess("Inlägget är borttaget.");
        return RedirectToAction("Details", "Topic", new { topicUrlName });
    }
}
