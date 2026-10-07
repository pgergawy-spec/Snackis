namespace SnackisAPI.Models;

public class Report
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Reason { get; set; } = "";
    public bool IsReviewed { get; set; }
    public string UserId { get; set; } = "";
    public User? User { get; set; }
    public Guid PostId { get; set; }
    public Post? Post { get; set; }

}
