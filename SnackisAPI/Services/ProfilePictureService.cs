using Microsoft.AspNetCore.Identity;
using SnackisAPI.Dtos;
using SnackisAPI.Exceptions;
using SnackisAPI.Mappings;
using SnackisAPI.Models;

namespace SnackisAPI.Services;

public class ProfilePictureService(UserManager<User> userManager, IWebHostEnvironment environment, ILogger<ProfilePictureService> logger) : IProfilePictureService
{// IWebHostEnvironment = info om var appen körs, används för sökvägen till wwwroot
    private const long MaxFileSizeInBytes = 2 * 1024 * 1024;
    private const int HeaderSizeInBytes = 12; // Så många byte i början av filen läser man för att se vilken filtyp det är.
    private const string PictureUrlFolder = "/uploads/profile-pictures";

    private static readonly (byte[] Signature, string Extension)[] PictureSignatures = // Varje filtyp börjar med en egen byte-följd (signatur) som vi jämför med.
    [
        (new byte[] { 0xFF, 0xD8, 0xFF }, ".jpg"),
        (new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, ".png"),
        ("GIF87a"u8.ToArray(), ".gif"),
        ("GIF89a"u8.ToArray(), ".gif")
    ];

    private string PictureFolderPath
        => Path.Combine(environment.WebRootPath ?? Path.Combine(environment.ContentRootPath, "wwwroot"), "uploads", "profile-pictures");

    public async Task<UserDto> UploadAsync(string userId, IFormFile pictureFile) // IFormFile = filen som användaren laddade upp.
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
        await using var stream = pictureFile.OpenReadStream();// OpenReadStream = öppnar den uppladdade filen som en ström, som läses bit för bit
        var bytesRead = await stream.ReadAtLeastAsync(header, HeaderSizeInBytes, throwOnEndOfStream: false);
        var fileStart = header.AsSpan(0, bytesRead);

        foreach (var (signature, extension) in PictureSignatures)
        {
            if (fileStart.StartsWith(signature))
                return extension;
        }

        var isWebP = bytesRead == HeaderSizeInBytes // WebP har en annan uppbyggnad: RIFF först och WEBP längre fram.
            && fileStart[..4].SequenceEqual("RIFF"u8)
            && fileStart[8..12].SequenceEqual("WEBP"u8);

        return isWebP ? ".webp" : null; // null betyder att filen inte är en tillåten bild.
    }

    private async Task SavePictureAsync(IFormFile pictureFile, string fileName)
    {
        Directory.CreateDirectory(PictureFolderPath);

        await using var pictureStream = pictureFile.OpenReadStream();
        await using var fileStream = File.Create(Path.Combine(PictureFolderPath, fileName));
        await pictureStream.CopyToAsync(fileStream); // Kopierar bilden från uppladdningen till filen på disk.
    }

    private void DeletePicture(string? pictureUrl)
    {
        if (pictureUrl is null || !pictureUrl.StartsWith(PictureUrlFolder))
            return;

        var picturePath = Path.Combine(PictureFolderPath, Path.GetFileName(pictureUrl)); // GetFileName tar bara filnamnet, så ingen kan peka ut en annan mapp.
        if (File.Exists(picturePath))
            File.Delete(picturePath);
    }
}
