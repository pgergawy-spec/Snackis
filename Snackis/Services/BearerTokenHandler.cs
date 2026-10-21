using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace Snackis.Services;

// Körs vid varje anrop till API:t: läser token ur cookien och lägger den i headern (Authorization: Bearer)
public class BearerTokenHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{// DelegatingHandler Sitter mellan MVC och Apiet Varje anrop passerar här, och sätter på token innan det går vidare
    public const string AccessTokenName = "access_token";

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {// Cancelation Avbryt-signal som skickas vidare, t.ex. om användaren stänger sidan
        var httpContext = httpContextAccessor.HttpContext;
        var accessToken = httpContext is null ? null : await httpContext.GetTokenAsync(AccessTokenName); //läser jwt i cookien.

        if (!string.IsNullOrEmpty(accessToken))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}
