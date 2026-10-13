namespace Snackis.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public string Message { get; set; } = "Något gick fel. Försök igen om en stund.";

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}