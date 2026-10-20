namespace Snackis.Models;

public class ReportViewModel
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Reason { get; set; } = "";
    public bool IsReviewed { get; set; }
    public string ReporterName { get; set; } = "";
    public Guid PostId { get; set; }
    public string PostTitle { get; set; } = "";
    public string PostAuthorName { get; set; } = "";
    public string TopicUrlName { get; set; } = "";
}
