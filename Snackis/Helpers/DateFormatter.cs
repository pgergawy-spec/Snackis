namespace Snackis.Helpers;

public static class DateFormatter
{
    private const string DisplayFormat = "yyyy-MM-dd HH:mm";

    private static readonly TimeZoneInfo SwedishTimeZone = FindSwedishTimeZone();

    public static string ToDisplayText(DateTime utcDate)
    {
        var utcDateWithKind = DateTime.SpecifyKind(utcDate, DateTimeKind.Utc);
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateWithKind, SwedishTimeZone).ToString(DisplayFormat);
    }

    private static TimeZoneInfo FindSwedishTimeZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById("Europe/Stockholm");
        }
        catch (TimeZoneNotFoundException)
        {
            return TimeZoneInfo.Utc;
        }
    }
}
