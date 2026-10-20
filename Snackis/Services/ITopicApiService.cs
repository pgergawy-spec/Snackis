using Snackis.Models;

namespace Snackis.Services;

public interface ITopicApiService
{
    Task<List<TopicViewModel>> GetAllTopicsAsync();
    Task<TopicViewModel> GetTopicAsync(string urlName);
    Task<TopicViewModel> CreateTopicAsync(CreateTopicViewModel createTopicViewModel);
}
