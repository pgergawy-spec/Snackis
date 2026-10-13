using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SnackisAPI.Helpers;

public static class UrlFormatter
{
    public static string ToCleanUrl(string text)
    {
        var withoutAccents = new string(text.Trim().ToLowerInvariant()
            .Normalize(NormalizationForm.FormD)
            .Where(character => CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
            .ToArray());

        return Regex.Replace(withoutAccents, "[^a-z0-9]+", "-").Trim('-');
    }
}