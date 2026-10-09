namespace SnackisAPI.Exceptions;

public class ForbiddenException(string message)
    : ApiException(message, StatusCodes.Status403Forbidden, "Inte tillåtet");
