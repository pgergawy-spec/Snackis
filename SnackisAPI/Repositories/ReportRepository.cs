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
    => await _dbSet.AsNoTracking()
        .Where(report => !report.IsReviewed)
        .Include(report => report.User)
        .Include(report => report.Post)
            .ThenInclude(post => post!.User)
        .Include(report => report.Post)
            .ThenInclude(post => post!.Topic)
        .OrderBy(report => report.Date).ToListAsync();
}
