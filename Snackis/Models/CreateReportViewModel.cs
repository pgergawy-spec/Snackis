using System.ComponentModel.DataAnnotations;
using Snackis.Helpers;

namespace Snackis.Models;

public class CreateReportViewModel
{
    public Guid PostId { get; set; }
    public string PostTitle { get; set; } = "";
    public string TopicUrlName { get; set; } = "";
    public string UrlTitle { get; set; } = "";

    [Required(ErrorMessage = "Beskriv varför du anmäler inlägget.")]
    [StringLength(FieldLengths.ReportReason, MinimumLength = 3, ErrorMessage = "Orsaken måste vara mellan {2} och {1} tecken.")]
    [Display(Name = "Orsak")]
    public string Reason { get; set; } = "";
}
