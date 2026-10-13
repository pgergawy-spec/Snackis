using Microsoft.EntityFrameworkCore;
using SnackisAPI.Data;
using SnackisAPI.Models;

namespace SnackisAPI.Repositories;

public class TopicRepository(SnackisDbContext context) : GenericRepository<Topic>(context), ITopicRepository
{

    public async Task<Topic?> GetByUrlNameAsync(string urlName)
        => await _dbSet.FirstOrDefaultAsync(topic => topic.UrlName == urlName);

    public async Task<IEnumerable<Topic>> GetSubTopicsAsync(Guid parentId)
        => await _dbSet.Where(topic => topic.ParentId == parentId).ToListAsync();
}
