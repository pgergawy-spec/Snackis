using Snackis.Models;

namespace Snackis.Services;

public class ReportApiService(IHttpClientFactory httpClientFactory, ILogger<ReportApiService> logger)
    : ApiServiceBase(httpClientFactory, logger), IReportApiService
{
    public Task<ReportViewModel> CreateReportAsync(CreateReportViewModel createReportViewModel)
        => PostAsync<ReportViewModel>("api/reports", new { createReportViewModel.PostId, createReportViewModel.Reason });

    public Task<List<ReportViewModel>> GetUnreviewedReportsAsync()
        => GetAsync<List<ReportViewModel>>("api/reports");

    public Task MarkAsReviewedAsync(Guid reportId)
        => PutAsync($"api/reports/{reportId}/review");
}
