using Snackis.Models;

namespace Snackis.Services;

public class PostApiService(IHttpClientFactory httpClientFactory, ILogger<PostApiService> logger)
    : ApiServiceBase(httpClientFactory, logger), IPostApiService
{
    public Task<List<PostViewModel>> GetPostsByTopicAsync(string topicUrlName)
        => GetAsync<List<PostViewModel>>($"api/topics/{Uri.EscapeDataString(topicUrlName)}/posts");
}
