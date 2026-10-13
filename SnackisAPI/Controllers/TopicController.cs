using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnackisAPI.Dtos;
using SnackisAPI.Services;

namespace SnackisAPI.Controllers;

[ApiController]
[Route("api/topics")]
public class TopicController(ITopicService topicService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TopicDto>>> GetTopicOverview()
        => Ok(await topicService.GetTopicOverviewAsync());

    [HttpGet("{urlName}")]
    public async Task<ActionResult<TopicDto>> GetByUrlName(string urlName)
        => Ok(await topicService.GetByUrlNameAsync(urlName));

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<TopicDto>> Create(CreateTopicDto createTopicDto)
    {
        var createdTopic = await topicService.CreateAsync(createTopicDto);
        return Ok(createdTopic);
    }
}
