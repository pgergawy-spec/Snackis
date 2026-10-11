using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Snackis.Exceptions;
using Snackis.Models;

namespace Snackis.Filters;

public class ApiExceptionFilter(IModelMetadataProvider modelMetadataProvider, ITempDataDictionaryFactory tempDataFactory) : IAsyncExceptionFilter
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        if (context.Exception is not ApiException apiException)
            return;

        context.Result = apiException.StatusCode == HttpStatusCode.Unauthorized
            ? await CreateSignOutResultAsync(context)
            : CreateErrorViewResult(context, apiException);

        context.ExceptionHandled = true;
    }

    private async Task<IActionResult> CreateSignOutResultAsync(ExceptionContext context)
    {
        await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        tempDataFactory.GetTempData(context.HttpContext)["ErrorMessage"] = "Din inloggning har gått ut. Logga in igen.";

        var request = context.HttpContext.Request;
        var returnUrl = HttpMethods.IsGet(request.Method) ? request.Path.ToString() : null;
        return new RedirectToActionResult("Login", "Account", new { returnUrl });
    }

    private ViewResult CreateErrorViewResult(ExceptionContext context, ApiException apiException)
    {
        var statusCode = (int)apiException.StatusCode >= StatusCodes.Status500InternalServerError
            ? StatusCodes.Status503ServiceUnavailable
            : (int)apiException.StatusCode;

        return new ViewResult
        {
            ViewName = "Error",
            StatusCode = statusCode,
            ViewData = new ViewDataDictionary<ErrorViewModel>(modelMetadataProvider, context.ModelState)
            {
                Model = new ErrorViewModel
                {
                    RequestId = context.HttpContext.TraceIdentifier,
                    Message = apiException.Message
                }
            }
        };
    }
}
