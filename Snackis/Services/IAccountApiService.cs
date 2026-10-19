using Snackis.Models;

namespace Snackis.Services;

public interface IAccountApiService
{
    public Task<AuthResultViewModel> LoginAsync(LoginViewModel loginViewModel);
}
