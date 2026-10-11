namespace Snackis.Models;

public class PostViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string UrlTitle { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime Date { get; set; }
    public Guid TopicId { get; set; }
    public string AuthorId { get; set; } = "";
    public string AuthorName { get; set; } = "";
    public string? AuthorPictureUrl { get; set; }
}