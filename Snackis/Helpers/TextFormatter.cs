namespace Snackis.Helpers;

public static class TextFormatter
{
    public static string Shorten(string text, int maximumLength)
        => text.Length <= maximumLength ? text : text[..maximumLength].TrimEnd() + "…";
}
