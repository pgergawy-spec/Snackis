namespace SnackisAPI.Exceptions;

public class UnauthorizedException(string message)
    : ApiException(message, StatusCodes.Status401Unauthorized, "Inte inloggad");
