using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snackis.Exceptions;
using Snackis.Models;
using Snackis.Services;

namespace Snackis.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin")]
public class AdminController(ITopicApiService topicApiService, IReportApiService reportApiService, ILogger<AdminController> logger) : SnackisControllerBase
{
    [HttpGet("amnen")]
    public async Task<IActionResult> Topics()
        => View(await WithMainTopicsAsync(new CreateTopicViewModel()));

    [HttpPost("amnen")]
    public async Task<IActionResult> Topics(CreateTopicViewModel createTopicViewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var createdTopic = await topicApiService.CreateTopicAsync(createTopicViewModel);
                logger.LogInformation("Admin {UserId} created topic {TopicId}", CurrentUserId, createdTopic.Id);

                ShowSuccess($"Ämnet \"{createdTopic.Name}\" är skapat.");
                return RedirectToAction(nameof(Topics));
            }
            catch (ApiException apiException) when (IsFormError(apiException))
            {
                ModelState.AddModelError(string.Empty, apiException.Message);
            }
        }

        return View(await WithMainTopicsAsync(createTopicViewModel));
    }

    [HttpGet("anmalningar")]
    public async Task<IActionResult> Reports()
        => View(await reportApiService.GetUnreviewedReportsAsync());

    [HttpPost("anmalningar/{reportId:guid}/granskad")]
    public async Task<IActionResult> MarkAsReviewed(Guid reportId)
    {
        await reportApiService.MarkAsReviewedAsync(reportId);
        logger.LogInformation("Admin {UserId} reviewed report {ReportId}", CurrentUserId, reportId);

        ShowSuccess("Anmälan är markerad som granskad.");
        return RedirectToAction(nameof(Reports));
    }

    private async Task<CreateTopicViewModel> WithMainTopicsAsync(CreateTopicViewModel createTopicViewModel)
    {
        createTopicViewModel.MainTopics = await topicApiService.GetAllTopicsAsync();
        return createTopicViewModel;
    }
}
