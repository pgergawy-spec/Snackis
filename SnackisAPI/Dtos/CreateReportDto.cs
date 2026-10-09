using System.ComponentModel.DataAnnotations;
using SnackisAPI.Helpers;

namespace SnackisAPI.Dtos;

public class CreateReportDto
{
    public Guid PostId { get; set; }

    [Required, StringLength(FieldLengths.ReportReason, MinimumLength = 3)]
    public string Reason { get; set; } = "";
}
