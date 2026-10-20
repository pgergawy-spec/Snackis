using System.ComponentModel.DataAnnotations;
using Snackis.Helpers;

namespace Snackis.Models;

public class CreateCommentViewModel
{
    public Guid PostId { get; set; }
    public string TopicUrlName { get; set; } = "";
    public string UrlTitle { get; set; } = "";

    [Required(ErrorMessage = "Skriv en kommentar.")]
    [StringLength(FieldLengths.CommentContent, MinimumLength = 1, ErrorMessage = "Kommentaren får vara högst {1} tecken.")]
    [Display(Name = "Kommentar")]
    public string Content { get; set; } = "";
}
