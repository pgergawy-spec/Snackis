using SnackisAPI.Models;

namespace SnackisAPI.Repositories;

public interface IReportRepository : IRepository<Report>
{
    Task<IEnumerable<Report>> GetUnreviewedAsync();
    Task<Report?> GetByIdWithDetailsAsync(Guid id);
    Task<bool> HasUserReportedPostAsync(Guid postId, string userId);
}
