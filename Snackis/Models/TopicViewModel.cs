namespace Snackis.Models;

public class TopicViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string UrlName { get; set; } = "";
    public Guid? ParentId { get; set; }
    public List<TopicViewModel> SubTopics { get; set; } = [];
}