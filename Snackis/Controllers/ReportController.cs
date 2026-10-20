using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snackis.Exceptions;
using Snackis.Models;
using Snackis.Services;

namespace Snackis.Controllers;

[Authorize]
public class ReportController(IReportApiService reportApiService, IPostApiService postApiService, ILogger<ReportController> logger) : SnackisControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Create(Guid postId)
    {
        var post = await postApiService.GetPostAsync(postId);

        return View(new CreateReportViewModel
        {
            PostId = post.Id,
            PostTitle = post.Title,
            TopicUrlName = post.TopicUrlName,
            UrlTitle = post.UrlTitle
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateReportViewModel createReportViewModel)
    {
        if (!ModelState.IsValid)
            return View(createReportViewModel);

        try
        {
            var createdReport = await reportApiService.CreateReportAsync(createReportViewModel);
            logger.LogInformation("User {UserId} reported post {PostId} as report {ReportId}", CurrentUserId, createReportViewModel.PostId, createdReport.Id);

            ShowSuccess("Tack, din anmälan är skickad. En administratör granskar inlägget.");
            return RedirectToPost(createReportViewModel.TopicUrlName, createReportViewModel.PostId, createReportViewModel.UrlTitle);
        }
        catch (ApiException apiException) when (IsFormError(apiException))
        {
            ModelState.AddModelError(string.Empty, apiException.Message);
            return View(createReportViewModel);
        }
    }
}
