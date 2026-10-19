using Snackis.Models;

namespace Snackis.Services;

public class AccountApiService(IHttpClientFactory httpClientFactory, ILogger<AccountApiService> logger)
    : ApiServiceBase(httpClientFactory, logger), IAccountApiService
{
    public Task<AuthResultViewModel> LoginAsync(LoginViewModel loginViewModel)
    => PostAsync<AuthResultViewModel>($"api/auth/login", new { loginViewModel.UserName, loginViewModel.Password });
}
