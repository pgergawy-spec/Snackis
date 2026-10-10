using Microsoft.EntityFrameworkCore;
using SnackisAPI.Data;
using SnackisAPI.Models;

namespace SnackisAPI.Repositories;

public class ReportRepository : GenericRepository<Report>, IReportRepository
{
    public ReportRepository(SnackisDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Report>> GetUnreviewedAsync()
        => await ReportsWithDetails()
            .AsNoTracking()
            .Where(report => !report.IsReviewed)
            .OrderBy(report => report.Date)
            .ToListAsync();

    public async Task<Report?> GetByIdWithDetailsAsync(Guid id)
        => await ReportsWithDetails()
            .FirstOrDefaultAsync(report => report.Id == id);

    public async Task<bool> HasUserReportedPostAsync(Guid postId, string userId)
        => await _dbSet.AnyAsync(report => report.PostId == postId && report.UserId == userId);

    private IQueryable<Report> ReportsWithDetails()
        => _dbSet
            .Include(report => report.User)
            .Include(report => report.Post)
                .ThenInclude(post => post!.User)
            .Include(report => report.Post)
                .ThenInclude(post => post!.Topic);
}
