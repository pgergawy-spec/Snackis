using Microsoft.EntityFrameworkCore;
using SnackisAPI.Data;
using SnackisAPI.Models;

namespace SnackisAPI.Repositories;

public class PostRepository : GenericRepository<Post>, IPostRepository
{
    public PostRepository(SnackisDbContext context) : base(context)
    {

    }
    public async Task<IEnumerable<Post>> GetByTopicAsync(Guid topicId)
    => await _dbSet.AsNoTracking()
        .Where(post => post.TopicId == topicId)
        .Include(post => post.User)
        .OrderByDescending(post => post.Date).ToListAsync();
    public async Task<Post?> GetByIdWithDetailsAsync(Guid id)
        => await _dbSet
            .Include(post => post.User)
            .Include(post => post.Topic)
                .ThenInclude(topic => topic!.Parent)
            .FirstOrDefaultAsync(post => post.Id == id);
}
