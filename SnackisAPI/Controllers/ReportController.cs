using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnackisAPI.Dtos;
using SnackisAPI.Services;

namespace SnackisAPI.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportController(IReportService reportService) : SnackisControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ReportDto>> Create(CreateReportDto createReportDto)
        => Ok(await reportService.CreateAsync(createReportDto, CurrentUserId));

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReportDto>>> GetUnreviewed()
        => Ok(await reportService.GetUnreviewedAsync());

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}/review")]
    public async Task<IActionResult> MarkAsReviewed(Guid id)
    {
        await reportService.MarkAsReviewedAsync(id);
        return NoContent();
    }
}
