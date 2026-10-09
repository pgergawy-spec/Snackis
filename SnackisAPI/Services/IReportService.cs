using SnackisAPI.Dtos;

namespace SnackisAPI.Services;

public interface IReportService
{
    Task<ReportDto> CreateAsync(CreateReportDto createReportDto, string userId);
    Task<IEnumerable<ReportDto>> GetUnreviewedAsync();
    Task MarkAsReviewedAsync(Guid id);
}
