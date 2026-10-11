namespace Snackis.Models;

public class TopicPageViewModel
{
    public TopicViewModel Topic { get; set; } = new();
    public TopicViewModel? ParentTopic { get; set; }
    public List<PostViewModel> Posts { get; set; } = [];
}
