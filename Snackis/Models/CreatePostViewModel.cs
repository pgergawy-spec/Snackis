using System.ComponentModel.DataAnnotations;
using Snackis.Helpers;

namespace Snackis.Models;

public class CreatePostViewModel
{
    public Guid TopicId { get; set; }
    public string TopicUrlName { get; set; } = "";
    public string TopicName { get; set; } = "";

    [Required(ErrorMessage = "Ange en rubrik.")]
    [StringLength(FieldLengths.PostTitle, MinimumLength = 2, ErrorMessage = "Rubriken måste vara mellan {2} och {1} tecken.")]
    [Display(Name = "Rubrik")]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "Skriv något i inlägget.")]
    [StringLength(FieldLengths.PostContent, MinimumLength = 2, ErrorMessage = "Inlägget måste vara mellan {2} och {1} tecken.")]
    [Display(Name = "Inlägg")]
    public string Content { get; set; } = "";
}
