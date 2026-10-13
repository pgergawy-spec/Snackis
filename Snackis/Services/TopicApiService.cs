using Snackis.Models;

namespace Snackis.Services;

public class TopicApiService(IHttpClientFactory httpClientFactory, ILogger<TopicApiService> logger)
    : ApiServiceBase(httpClientFactory, logger), ITopicApiService
{
    public Task<List<TopicViewModel>> GetAllTopicsAsync()
        => GetAsync<List<TopicViewModel>>("api/topics");

    public Task<TopicViewModel> GetTopicAsync(string urlName)
        => GetAsync<TopicViewModel>($"api/topics/{Uri.EscapeDataString(urlName)}");
}
