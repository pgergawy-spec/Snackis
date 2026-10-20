using Snackis.Models;

namespace Snackis.Services;

public interface IReportApiService
{
    Task<ReportViewModel> CreateReportAsync(CreateReportViewModel createReportViewModel);
    Task<List<ReportViewModel>> GetUnreviewedReportsAsync();
    Task MarkAsReviewedAsync(Guid reportId);
}
