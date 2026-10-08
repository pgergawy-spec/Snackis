using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SnackisAPI.Exceptions;

public class ExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ExceptionHandler> _logger;

    public ExceptionHandler(ILogger<ExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, errorTitle) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Hittades inte"),
            ConflictException => (StatusCodes.Status409Conflict, "Finns redan"),
            BadRequestException => (StatusCodes.Status400BadRequest, "Ogiltig begäran"),
            Exception => (StatusCodes.Status500InternalServerError, "Något gick fel")
        };

        var isUnexpectedError = statusCode == StatusCodes.Status500InternalServerError;

        if (isUnexpectedError)
            _logger.LogError(exception, "Unexpected error in {Path}", httpContext.Request.Path);
        else
            _logger.LogWarning("{ErrorTitle}: {ErrorMessage}", errorTitle, exception.Message);

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = statusCode,
            Title = errorTitle,
            Detail = isUnexpectedError ? null : exception.Message
        }, cancellationToken);

        return true;
    }
}