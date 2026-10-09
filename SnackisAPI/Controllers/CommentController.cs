using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnackisAPI.Dtos;
using SnackisAPI.Services;

namespace SnackisAPI.Controllers;

[ApiController]
public class CommentController(ICommentService commentService) : SnackisControllerBase
{
    [HttpGet("api/posts/{postId:guid}/comments")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetByPost(Guid postId)
        => Ok(await commentService.GetByPostAsync(postId));

    [Authorize]
    [HttpPost("api/comments")]
    public async Task<ActionResult<CommentDto>> Create(CreateCommentDto createCommentDto)
        => Ok(await commentService.CreateAsync(createCommentDto, CurrentUserId));

    [Authorize]
    [HttpDelete("api/comments/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await commentService.DeleteAsync(id, CurrentUserId, IsCurrentUserAdmin);
        return NoContent();
    }
}
