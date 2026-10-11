using Snackis.Exceptions;
using Snackis.Models;
using System.Net;
using System.Text.Json;

namespace Snackis.Services;

public abstract class ApiServiceBase(IHttpClientFactory httpClientFactory, ILogger logger)
{
    public const string HttpClientName = "SnackisAPI";

    private readonly HttpClient httpClient = httpClientFactory.CreateClient(HttpClientName);

    protected async Task<TResult> GetAsync<TResult>(string url)
        => await ReadResultAsync<TResult>(await SendRequestAsync(HttpMethod.Get, url));

    protected async Task<TResult> PostAsync<TResult>(string url, object body) //object = samma idea som T men mindre typsäkert.
        => await ReadResultAsync<TResult>(await SendRequestAsync(HttpMethod.Post, url, JsonContent.Create(body)));

    protected async Task<TResult> SendFormAsync<TResult>(HttpMethod method, string url, HttpContent form)
        => await ReadResultAsync<TResult>(await SendRequestAsync(method, url, form));

    protected async Task PutAsync(string url)
    {
        using var response = await SendRequestAsync(HttpMethod.Put, url);
    }

    protected async Task DeleteAsync(string url)
    {
        using var response = await SendRequestAsync(HttpMethod.Delete, url);
    }

    private async Task<HttpResponseMessage> SendRequestAsync(HttpMethod method, string url, HttpContent? content = null)
    {
        using var request = new HttpRequestMessage(method, url) { Content = content };

        HttpResponseMessage response;
        try
        {
            response = await httpClient.SendAsync(request);
        }
        catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException)
        {
            logger.LogError(exception, "Could not reach the API for {Method} {Url}", method, url);
            throw new ApiException(HttpStatusCode.ServiceUnavailable, "Forumets server svarar inte just nu. Försök igen om en stund.");
        }

        if (response.IsSuccessStatusCode)
            return response;

        var apiException = await CreateApiExceptionAsync(response);
        response.Dispose();
        throw apiException;
    }

    private static async Task<TResult> ReadResultAsync<TResult>(HttpResponseMessage response)
    {
        using (response)
        {
            return await response.Content.ReadFromJsonAsync<TResult>()
                ?? throw new ApiException(HttpStatusCode.BadGateway, "Servern gav ett tomt svar.");
        }
    }

    private async Task<ApiException> CreateApiExceptionAsync(HttpResponseMessage response)
    {
        var message = await ReadErrorMessageAsync(response) ?? GetDefaultMessage(response.StatusCode);

        logger.LogWarning("API answered {StatusCode} for {Method} {Path}: {Message}",
            (int)response.StatusCode, response.RequestMessage?.Method, response.RequestMessage?.RequestUri?.AbsolutePath, message);

        return new ApiException(response.StatusCode, message);
    }

    private static async Task<string?> ReadErrorMessageAsync(HttpResponseMessage response)
    {
        try
        {
            var problem = await response.Content.ReadFromJsonAsync<ApiProblemViewModel>();

            if (problem?.Errors is { Count: > 0 })
                return string.Join(" ", problem.Errors.Values.SelectMany(messages => messages));

            return problem?.Detail ?? problem?.Title;
        }
        catch (Exception exception) when (exception is JsonException or NotSupportedException)
        {
            return null;
        }
    }

    private static string GetDefaultMessage(HttpStatusCode statusCode) => statusCode switch
    {
        HttpStatusCode.Unauthorized => "Du måste logga in.",
        HttpStatusCode.Forbidden => "Du har inte behörighet att göra det här.",
        HttpStatusCode.NotFound => "Det du letar efter finns inte.",
        >= HttpStatusCode.InternalServerError => "Servern har problem just nu. Försök igen om en stund.",
        _ => "Något gick fel." // _ = övrigt.
    };
}