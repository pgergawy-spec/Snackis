using SnackisAPI.Dtos;

namespace SnackisAPI.Services;

public interface IPostService
{
    Task<IEnumerable<PostDto>> GetByTopicAsync(string topicUrlName);
    Task<PostDetailsDto> GetDetailsAsync(Guid id);
    Task<PostDetailsDto> CreateAsync(CreatePostDto createPostDto, string userId);
    Task DeleteAsync(Guid id, string userId, bool isAdmin);
}
