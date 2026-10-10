using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SnackisAPI.Exceptions;

public class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, errorTitle, errorDetail) = exception switch
        {
            ApiException apiException => (apiException.StatusCode, apiException.Title, exception.Message),
            BadHttpRequestException badRequestException => (badRequestException.StatusCode, "Ogiltig begäran", "Förfrågan har fel format."),
            Exception => (StatusCodes.Status500InternalServerError, "Något gick fel", (string?)null)
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Unexpected error in {Path}", httpContext.Request.Path);
        else
            logger.LogWarning("{ErrorTitle}: {ErrorMessage}", errorTitle, exception.Message);

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = statusCode,
            Title = errorTitle,
            Detail = errorDetail
        }, cancellationToken);

        return true;
    }
}
