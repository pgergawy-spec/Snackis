namespace SnackisAPI.Dtos;

public class TopicDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string UrlName { get; set; } = "";
    public Guid? ParentId { get; set; }
    public List<TopicDto> SubTopics { get; set; } = [];
}
