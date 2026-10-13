namespace Snackis.Models;

public class ApiProblemViewModel
{
    public string? Title { get; set; }
    public string? Detail { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
}