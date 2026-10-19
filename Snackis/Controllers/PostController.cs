using Microsoft.AspNetCore.Mvc;
using Snackis.Models;
using Snackis.Services;

namespace Snackis.Controllers;

public class PostController(IPostApiService postApiService, ICommentApiService commentApiService) : SnackisControllerBase
{
    [HttpGet("forum/{topicUrlName}/{postId:guid}/{urlTitle?}")]
    public async Task<IActionResult> Details(Guid postId)
    {
        var post = await postApiService.GetPostAsync(postId);
        var comments = await commentApiService.GetCommentsAsync(postId);

        return View(new PostPageViewModel
        {
            Post = post,
            Comments = comments
        });
    }
}