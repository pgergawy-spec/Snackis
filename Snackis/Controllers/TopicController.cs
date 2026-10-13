using Microsoft.AspNetCore.Mvc;
using Snackis.Models;
using Snackis.Services;

namespace Snackis.Controllers;

public class TopicController(ITopicApiService topicApiService, IPostApiService postApiService) : SnackisControllerBase
{
    [HttpGet("forum/{topicUrlName}")]
    public async Task<IActionResult> Details(string topicUrlName)
    {
        var topic = await topicApiService.GetTopicAsync(topicUrlName);

        return View(new TopicPageViewModel
        {
            Topic = topic,
            ParentTopic = await FindParentTopicAsync(topic),
            Posts = await postApiService.GetPostsByTopicAsync(topicUrlName)
        });
    }

    private async Task<TopicViewModel?> FindParentTopicAsync(TopicViewModel topic)
    {
        if (topic.ParentId is null)
            return null;

        var mainTopics = await topicApiService.GetAllTopicsAsync();
        return mainTopics.FirstOrDefault(mainTopic => mainTopic.Id == topic.ParentId);
    }
}
