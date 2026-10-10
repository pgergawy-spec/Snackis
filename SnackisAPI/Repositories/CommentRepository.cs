using Microsoft.EntityFrameworkCore;
using SnackisAPI.Data;
using SnackisAPI.Models;

namespace SnackisAPI.Repositories;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(SnackisDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Comment>> GetByPostAsync(Guid postId)
        => await _dbSet.AsNoTracking()
            .Where(comment => comment.PostId == postId)
            .Include(comment => comment.User)
            .OrderBy(comment => comment.Date)
            .ToListAsync();

    public async Task<Comment?> GetByIdWithAuthorAsync(Guid id)
        => await _dbSet
            .Include(comment => comment.User)
            .FirstOrDefaultAsync(comment => comment.Id == id);
}
