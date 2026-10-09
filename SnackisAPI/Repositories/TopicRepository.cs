using Microsoft.EntityFrameworkCore;
using SnackisAPI.Data;
using SnackisAPI.Models;

namespace SnackisAPI.Repositories;

public class TopicRepository : GenericRepository<Topic>, ITopicRepository
{
    public TopicRepository(SnackisDbContext context) : base(context)
    {

    }

    public async Task<Topic?> GetByUrlNameAsync(string urlName)
        => await _dbSet.FirstOrDefaultAsync(topic => topic.UrlName == urlName);

    public async Task<IEnumerable<Topic>> GetMainTopicsAsync()
        => await _dbSet
        .Where(topic => topic.ParentId == null).ToListAsync();

    public async Task<IEnumerable<Topic>> GetSubTopicsAsync(Guid parentId)
        => await _dbSet.Where(topic => topic.ParentId == parentId).ToListAsync();
}
