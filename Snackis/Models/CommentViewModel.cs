namespace Snackis.Models;

public class CommentViewModel
{
    public Guid Id { get; set; }
    public string Content { get; set; } = "";
    public DateTime Date { get; set; }
    public Guid PostId { get; set; }
    public string AuthorId { get; set; } = "";
    public string AuthorName { get; set; } = "";
    public string? AuthorPictureUrl { get; set; }
}