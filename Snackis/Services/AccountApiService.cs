using System.Net.Http.Headers;
using Snackis.Models;

namespace Snackis.Services;

public class AccountApiService(IHttpClientFactory httpClientFactory, ILogger<AccountApiService> logger)
    : ApiServiceBase(httpClientFactory, logger), IAccountApiService
{
    public Task<AuthResultViewModel> LoginAsync(LoginViewModel loginViewModel)
        => PostAsync<AuthResultViewModel>("api/auth/login", new { loginViewModel.UserName, loginViewModel.Password });

    public Task<UserViewModel> RegisterAsync(RegisterViewModel registerViewModel)
        => PostAsync<UserViewModel>("api/auth/register", new { registerViewModel.UserName, registerViewModel.Email, registerViewModel.Password });

    public Task<UserViewModel> GetCurrentUserAsync()
        => GetAsync<UserViewModel>("api/users/me");

    public async Task<UserViewModel> UploadProfilePictureAsync(IFormFile profilePicture) // Skickar bilden som formulärdata (multipart) till API:t.
    {
        using var form = new MultipartFormDataContent(); // Formuläret som innehåller filen.
        using var fileContent = new StreamContent(profilePicture.OpenReadStream()); // Läser filen som en ström, så den inte laddas hela in i minnet.

        if (MediaTypeHeaderValue.TryParse(profilePicture.ContentType, out var contentType)) // Tar med filens typ (t.ex. image/png) om den går att läsa.
            fileContent.Headers.ContentType = contentType;

        form.Add(fileContent, "profilePicture", profilePicture.FileName); // Namnet profilePicture måste matcha parametern i API:ts UserController.
        return await SendFormAsync<UserViewModel>(HttpMethod.Put, "api/users/me/profile-picture", form); // PUT till API:t, svaret är den uppdaterade användaren.
    }
}
