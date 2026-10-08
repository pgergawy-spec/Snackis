using SnackisAPI.Models;

namespace SnackisAPI.Repositories;

public interface ITopicRepository : IRepository<Topic>
{
    Task<Topic?> GetBySlugAsync(string slug);
    Task<IEnumerable<Topic>> GetSubTopicsAsync(Guid parentId);
    Task<IEnumerable<Topic>> GetMainTopicsAsync();
}
