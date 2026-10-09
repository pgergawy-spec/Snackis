using SnackisAPI.Dtos;
using SnackisAPI.Exceptions;
using SnackisAPI.Mappings;
using SnackisAPI.Repositories;

namespace SnackisAPI.Services;

public class TopicService(ITopicRepository topicRepository, ILogger<TopicService> logger) : ITopicService
{
    public async Task<IEnumerable<TopicDto>> GetTopicOverviewAsync()
        => (await topicRepository.GetAllAsync()).ToTopicOverview();

    public async Task<TopicDto> GetByUrlNameAsync(string urlName)
    {
        var topic = await topicRepository.GetByUrlNameAsync(urlName)
            ?? throw new NotFoundException("Ämnet finns inte.");

        var subTopics = await topicRepository.GetSubTopicsAsync(topic.Id);
        return topic.ToDto(subTopics);
    }

    public async Task<TopicDto> CreateAsync(CreateTopicDto createTopicDto)
    {
        var topic = createTopicDto.ToEntity();

        if (string.IsNullOrEmpty(topic.UrlName))
            throw new BadRequestException("Namnet måste innehålla minst en bokstav eller siffra.");

        await EnsureParentIsMainTopicAsync(createTopicDto.ParentId);

        if (await topicRepository.GetByUrlNameAsync(topic.UrlName) is not null)
            throw new ConflictException($"Det finns redan ett ämne med url-namnet '{topic.UrlName}'.");

        await topicRepository.AddAsync(topic);
        logger.LogInformation("Topic {TopicId} created with url name {UrlName}", topic.Id, topic.UrlName);

        return topic.ToDto();
    }

    private async Task EnsureParentIsMainTopicAsync(Guid? parentId)
    {
        if (parentId is null)
            return;

        var parentTopic = await topicRepository.GetByIdAsync(parentId.Value)
            ?? throw new NotFoundException("Huvudämnet finns inte.");

        if (parentTopic.ParentId != null)
            throw new BadRequestException("Ett underämne kan inte ha egna underämnen.");
    }
}
