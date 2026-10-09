using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SnackisAPI.Data;

public class UtcDateTimeConverter() : ValueConverter<DateTime, DateTime>(
    dateTime => dateTime,
    dateTime => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
