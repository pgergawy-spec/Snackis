using SnackisAPI.Models;

namespace SnackisAPI.Repositories;

public interface IReportRepository : IRepository<Report>
{
    Task<IEnumerable<Report>> GetUnreviewedAsync();
}
