using SnackisAPI.Models;

namespace SnackisAPI.Repositories;

public interface IPostRepository : IRepository<Post>
{
    Task<IEnumerable<Post>> GetByTopicAsync(Guid topicId);
    Task<Post?> GetByIdWithDetailsAsync(Guid id);
}
