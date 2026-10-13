using SnackisAPI.Dtos;

namespace SnackisAPI.Services;

public interface IProfilePictureService
{
    Task<UserDto> UploadAsync(string userId, IFormFile pictureFile);
}
