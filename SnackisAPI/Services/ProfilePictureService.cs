using Microsoft.AspNetCore.Identity;
using SnackisAPI.Dtos;
using SnackisAPI.Exceptions;
using SnackisAPI.Mappings;
using SnackisAPI.Models;

namespace SnackisAPI.Services;

public class ProfilePictureService(UserManager<User> userManager, IWebHostEnvironment environment, ILogger<ProfilePictureService> logger) : IProfilePictureService
{
    private const long MaxFileSizeInBytes = 2 * 1024 * 1024;
    private const int HeaderSizeInBytes = 12;
    private const string PictureUrlFolder = "/uploads/profile-pictures";

    private static readonly (byte[] Signature, string Extension)[] PictureSignatures =
    [
        (new byte[] { 0xFF, 0xD8, 0xFF }, ".jpg"),
        (new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, ".png"),
        ("GIF87a"u8.ToArray(), ".gif"),
        ("GIF89a"u8.ToArray(), ".gif")
    ];

    private string PictureFolderPath
        => Path.Combine(environment.WebRootPath ?? Path.Combine(environment.ContentRootPath, "wwwroot"), "uploads", "profile-pictures");

    public async Task<UserDto> UploadAsync(string userId, IFormFile pictureFile)
    {
        var user = await userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("Användaren finns inte.");

        if (pictureFile.Length == 0 || pictureFile.Length > MaxFileSizeInBytes)
            throw new BadRequestException("Bilden måste vara större än 0 och högst 2 MB.");

        var fileExtension = await DetectPictureExtensionAsync(pictureFile)
            ?? throw new BadRequestException("Bilden måste vara en JPG-, PNG-, GIF- eller WebP-fil.");

        var newFileName = $"{Guid.NewGuid():N}{fileExtension}";
        await SavePictureAsync(pictureFile, newFileName);

        var oldPictureUrl = user.ProfilePictureUrl;
        user.ProfilePictureUrl = $"{PictureUrlFolder}/{newFileName}";
        await userManager.UpdateAsync(user);
        DeletePicture(oldPictureUrl);

        logger.LogInformation("User {UserId} uploaded a new profile picture {FileName}", userId, newFileName);
        return EntityMappings.ToDto(user, await userManager.GetRolesAsync(user));
    }

    private static async Task<string?> DetectPictureExtensionAsync(IFormFile pictureFile)
    {
        var header = new byte[HeaderSizeInBytes];
        await using var stream = pictureFile.OpenReadStream();
        var bytesRead = await stream.ReadAtLeastAsync(header, HeaderSizeInBytes, throwOnEndOfStream: false);
        var fileStart = header.AsSpan(0, bytesRead);

        foreach (var (signature, extension) in PictureSignatures)
        {
            if (fileStart.StartsWith(signature))
                return extension;
        }

        var isWebP = bytesRead == HeaderSizeInBytes
            && fileStart[..4].SequenceEqual("RIFF"u8)
            && fileStart[8..12].SequenceEqual("WEBP"u8);

        return isWebP ? ".webp" : null;
    }

    private async Task SavePictureAsync(IFormFile pictureFile, string fileName)
    {
        Directory.CreateDirectory(PictureFolderPath);

        await using var pictureStream = pictureFile.OpenReadStream();
        await using var fileStream = File.Create(Path.Combine(PictureFolderPath, fileName));
        await pictureStream.CopyToAsync(fileStream);
    }

    private void DeletePicture(string? pictureUrl)
    {
        if (pictureUrl is null || !pictureUrl.StartsWith(PictureUrlFolder))
            return;

        var picturePath = Path.Combine(PictureFolderPath, Path.GetFileName(pictureUrl));
        if (File.Exists(picturePath))
            File.Delete(picturePath);
    }
}
