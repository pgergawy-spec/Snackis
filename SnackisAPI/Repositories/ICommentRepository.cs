using SnackisAPI.Models;

namespace SnackisAPI.Repositories;

public interface ICommentRepository : IRepository<Comment>
{
    Task<IEnumerable<Comment>> GetByPostAsync(Guid postId);
}
