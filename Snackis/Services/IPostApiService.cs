using Snackis.Models;

namespace Snackis.Services;

public interface IPostApiService
{
    Task<List<PostViewModel>> GetPostsByTopicAsync(string topicUrlName);

    Task<PostDetailsViewModel> GetPostAsync(Guid postId);
}
