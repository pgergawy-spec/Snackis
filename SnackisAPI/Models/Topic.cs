namespace SnackisAPI.Models;

public class Topic
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public Guid? ParentId { get; set; } // Self-referencing FK till Topic. Nullable eftersom ämnen utan förälder saknar värde att sätta här.
    public Topic? Parent { get; set; }
}
