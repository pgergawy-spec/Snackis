namespace SnackisAPI.Exceptions;

public class ConflictException(string message)
    : ApiException(message, StatusCodes.Status409Conflict, "Finns redan");
