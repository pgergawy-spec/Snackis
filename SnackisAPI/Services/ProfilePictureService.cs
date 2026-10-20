using Microsoft.AspNetCore.Identity;
using SnackisAPI.Dtos;
using SnackisAPI.Exceptions;
using SnackisAPI.Mappings;
using SnackisAPI.Models;

namespace SnackisAPI.Services;

public class ProfilePictureService(UserManager<User> userManager, IWebHostEnvironment environment, ILogger<ProfilePictureService> logger) : IProfilePictureService // Sköter uppladdning av profilbilder: kontrollerar, sparar och byter bild.
{
    private const long MaxFileSizeInBytes = 2 * 1024 * 1024; // Största tillåtna bild: 2 MB.
    private const int HeaderSizeInBytes = 12; // Så många byte i början av filen läser vi för att se vilken filtyp det är.
    private const string PictureUrlFolder = "/uploads/profile-pictures"; // Adressen som bilderna nås på i webbläsaren.

    private static readonly (byte[] Signature, string Extension)[] PictureSignatures = // Varje filtyp börjar med en egen byte-följd (signatur) som vi jämför med.
    [
        (new byte[] { 0xFF, 0xD8, 0xFF }, ".jpg"),
        (new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, ".png"),
        ("GIF87a"u8.ToArray(), ".gif"),
        ("GIF89a"u8.ToArray(), ".gif")
    ];

    private string PictureFolderPath // Mappen på servern där bilderna sparas (wwwroot/uploads/profile-pictures).
        => Path.Combine(environment.WebRootPath ?? Path.Combine(environment.ContentRootPath, "wwwroot"), "uploads", "profile-pictures");

    public async Task<UserDto> UploadAsync(string userId, IFormFile pictureFile) // IFormFile = filen som användaren laddade upp.
    {
        var user = await userManager.FindByIdAsync(userId) // Hämtar användaren som är inloggad.
            ?? throw new NotFoundException("Användaren finns inte.");

        if (pictureFile.Length == 0 || pictureFile.Length > MaxFileSizeInBytes) // Nekar tomma filer och filer över 2 MB.
            throw new BadRequestException("Bilden måste vara större än 0 och högst 2 MB.");

        var fileExtension = await DetectPictureExtensionAsync(pictureFile) // Läser filens innehåll, inte bara namnet, så att man inte kan smuggla in andra filer.
            ?? throw new BadRequestException("Bilden måste vara en JPG-, PNG-, GIF- eller WebP-fil.");

        var newFileName = $"{Guid.NewGuid():N}{fileExtension}"; // Nytt slumpat filnamn, så att bilder aldrig skriver över varandra.
        await SavePictureAsync(pictureFile, newFileName); // Sparar filen på disk.

        var oldPictureUrl = user.ProfilePictureUrl; // Kommer ihåg den gamla bilden så att den kan tas bort.
        user.ProfilePictureUrl = $"{PictureUrlFolder}/{newFileName}"; // Sparar adressen till den nya bilden på användaren.
        await userManager.UpdateAsync(user); // Skriver ändringen till databasen.
        DeletePicture(oldPictureUrl); // Tar bort den gamla bilden från disk.

        logger.LogInformation("User {UserId} uploaded a new profile picture {FileName}", userId, newFileName);
        return EntityMappings.ToDto(user, await userManager.GetRolesAsync(user)); // Gör om användaren till en DTO som skickas tillbaka till klienten.
    }

    private static async Task<string?> DetectPictureExtensionAsync(IFormFile pictureFile) // Returnerar filändelsen om det är en riktig bild, annars null.
    {
        var header = new byte[HeaderSizeInBytes]; // Plats för filens första byte.
        await using var stream = pictureFile.OpenReadStream();
        var bytesRead = await stream.ReadAtLeastAsync(header, HeaderSizeInBytes, throwOnEndOfStream: false); // Läser filens början (kortare filer ger färre byte).
        var fileStart = header.AsSpan(0, bytesRead);

        foreach (var (signature, extension) in PictureSignatures)
        {
            if (fileStart.StartsWith(signature)) // Börjar filen med signaturen för JPG, PNG eller GIF?
                return extension;
        }

        var isWebP = bytesRead == HeaderSizeInBytes // WebP har en annan uppbyggnad: RIFF först och WEBP längre fram.
            && fileStart[..4].SequenceEqual("RIFF"u8)
            && fileStart[8..12].SequenceEqual("WEBP"u8);

        return isWebP ? ".webp" : null; // null betyder att filen inte är en tillåten bild.
    }

    private async Task SavePictureAsync(IFormFile pictureFile, string fileName) // Skriver den uppladdade filen till bildmappen.
    {
        Directory.CreateDirectory(PictureFolderPath); // Skapar mappen om den inte finns (gör inget om den redan finns).

        await using var pictureStream = pictureFile.OpenReadStream();
        await using var fileStream = File.Create(Path.Combine(PictureFolderPath, fileName));
        await pictureStream.CopyToAsync(fileStream); // Kopierar bilden från uppladdningen till filen på disk.
    }

    private void DeletePicture(string? pictureUrl) // Tar bort en gammal profilbild från disk.
    {
        if (pictureUrl is null || !pictureUrl.StartsWith(PictureUrlFolder)) // Rör bara filer i bildmappen, aldrig något annat.
            return;

        var picturePath = Path.Combine(PictureFolderPath, Path.GetFileName(pictureUrl)); // GetFileName tar bara filnamnet, så ingen kan peka ut en annan mapp.
        if (File.Exists(picturePath))
            File.Delete(picturePath);
    }
}
