using SnackisAPI.Dtos;
using SnackisAPI.Exceptions;
using SnackisAPI.Helpers;
using SnackisAPI.Mappings;
using SnackisAPI.Repositories;

namespace SnackisAPI.Services;

public class PostService(IPostRepository postRepository, ITopicRepository topicRepository, ILogger<PostService> logger) : IPostService
{
    public async Task<IEnumerable<PostDto>> GetByTopicAsync(string topicUrlName)
    {
        var topic = await topicRepository.GetByUrlNameAsync(topicUrlName)
            ?? throw new NotFoundException("Ämnet finns inte.");

        return EntityMappings.ToDtos(await postRepository.GetByTopicAsync(topic.Id));
    }

    public async Task<PostDetailsDto> GetDetailsAsync(Guid id)
    {
        var post = await postRepository.GetByIdWithDetailsAsync(id)
            ?? throw new NotFoundException("Inlägget finns inte.");

        return EntityMappings.ToDetailsDto(post);
    }

    public async Task<PostDetailsDto> CreateAsync(CreatePostDto createPostDto, string userId)
    {
        var topic = await topicRepository.GetByIdAsync(createPostDto.TopicId)
            ?? throw new NotFoundException("Ämnet finns inte.");

        if (topic.ParentId == null)
            throw new BadRequestException("Inlägg kan bara skapas i ett underämne.");

        var post = EntityMappings.ToEntity(createPostDto, userId);
        await postRepository.AddAsync(post);
        logger.LogInformation("Post {PostId} created by user {UserId} in topic {TopicId}", post.Id, userId, topic.Id);

        return await GetDetailsAsync(post.Id);
    }

    public async Task DeleteAsync(Guid id, string userId, bool isAdmin)
    {
        var post = await postRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Inlägget finns inte.");

        AccessRules.EnsureOwnerOrAdmin(post.UserId, userId, isAdmin, "Du kan bara ta bort dina egna inlägg.");

        await postRepository.DeleteAsync(post);
        logger.LogInformation("Post {PostId} deleted by user {UserId}", post.Id, userId);
    }
}
