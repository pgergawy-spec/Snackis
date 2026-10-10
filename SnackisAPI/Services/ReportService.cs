using SnackisAPI.Dtos;
using SnackisAPI.Exceptions;
using SnackisAPI.Mappings;
using SnackisAPI.Repositories;

namespace SnackisAPI.Services;

public class ReportService(IReportRepository reportRepository, IPostRepository postRepository, ILogger<ReportService> logger) : IReportService
{
    public async Task<ReportDto> CreateAsync(CreateReportDto createReportDto, string userId)
    {
        await postRepository.EnsureExistsAsync(createReportDto.PostId);

        if (await reportRepository.HasUserReportedPostAsync(createReportDto.PostId, userId))
            throw new ConflictException("Du har redan anmält det här inlägget.");

        var report = createReportDto.ToEntity(userId);
        await reportRepository.AddAsync(report);
        logger.LogInformation("Report {ReportId} created by user {UserId} on post {PostId}", report.Id, userId, report.PostId);

        var createdReport = await reportRepository.GetByIdWithDetailsAsync(report.Id)
            ?? throw new NotFoundException("Anmälan finns inte.");

        return createdReport.ToDto();
    }

    public async Task<IEnumerable<ReportDto>> GetUnreviewedAsync()
        => (await reportRepository.GetUnreviewedAsync()).ToDtos();

    public async Task MarkAsReviewedAsync(Guid id)
    {
        var report = await reportRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Anmälan finns inte.");

        report.IsReviewed = true;
        await reportRepository.UpdateAsync(report);
        logger.LogInformation("Report {ReportId} marked as reviewed", report.Id);
    }
}
