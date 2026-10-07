namespace SnackisAPI.Models;

public class Post
{
    public Guid Id { get; set; }
    public string Content { get; set; } = "";
    public string Title { get; set; } = "";
    public DateTime Date { get; set; }
    public string UserId { get; set; } = "";
    public User? User { get; set; }
    public Guid TopicId { get; set; }
    public Topic? Topic { get; set; }
}
