using SnackisAPI.Dtos;

namespace SnackisAPI.Services;

public interface ITopicService
{
    Task<IEnumerable<TopicDto>> GetTopicOverviewAsync();
    Task<TopicDto> GetByUrlNameAsync(string urlName);
    Task<TopicDto> CreateAsync(CreateTopicDto createTopicDto);
}
