using Snackis.Models;

namespace Snackis.Services;

public interface IPostApiService
{
    Task<List<PostViewModel>> GetPostsByTopicAsync(string topicUrlName);
    Task<PostDetailsViewModel> GetPostAsync(Guid postId);
    Task<PostViewModel> CreatePostAsync(CreatePostViewModel createPostViewModel);
    Task DeletePostAsync(Guid postId);
}
