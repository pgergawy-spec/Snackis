using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnackisAPI.Dtos;
using SnackisAPI.Services;

namespace SnackisAPI.Controllers;

[ApiController]
public class PostController(IPostService postService) : SnackisControllerBase
{
    [HttpGet("api/topics/{topicUrlName}/posts")]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetByTopic(string topicUrlName)
        => Ok(await postService.GetByTopicAsync(topicUrlName));

    [HttpGet("api/posts/{id:guid}")]
    public async Task<ActionResult<PostDetailsDto>> GetDetails(Guid id)
        => Ok(await postService.GetDetailsAsync(id));

    [Authorize]
    [HttpPost("api/posts")]
    public async Task<ActionResult<PostDetailsDto>> Create(CreatePostDto createPostDto)
        => Ok(await postService.CreateAsync(createPostDto, CurrentUserId));

    [Authorize]
    [HttpDelete("api/posts/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await postService.DeleteAsync(id, CurrentUserId, IsCurrentUserAdmin);
        return NoContent();
    }
}
