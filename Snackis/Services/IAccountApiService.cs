using Snackis.Models;

namespace Snackis.Services;

public interface IAccountApiService
{
    Task<AuthResultViewModel> LoginAsync(LoginViewModel loginViewModel);
    Task<UserViewModel> RegisterAsync(RegisterViewModel registerViewModel);
    Task<UserViewModel> GetCurrentUserAsync();
    Task<UserViewModel> UploadProfilePictureAsync(IFormFile profilePicture);
}
