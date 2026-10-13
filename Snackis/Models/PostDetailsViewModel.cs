namespace Snackis.Models;

public class PostDetailsViewModel : PostViewModel
{
    public string TopicName { get; set; } = "";
    public string TopicUrlName { get; set; } = "";
    public string? ParentTopicName { get; set; }
    public string? ParentTopicUrlName { get; set; }
}