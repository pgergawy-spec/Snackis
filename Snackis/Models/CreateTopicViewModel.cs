using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Snackis.Helpers;

namespace Snackis.Models;

public class CreateTopicViewModel
{
    [Required(ErrorMessage = "Ange ett namn.")]
    [StringLength(FieldLengths.TopicName, MinimumLength = 2, ErrorMessage = "Namnet måste vara mellan {2} och {1} tecken.")]
    [Display(Name = "Namn")]
    public string Name { get; set; } = "";

    [Display(Name = "Placera under")]
    public Guid? ParentId { get; set; }

    [BindNever]
    public List<TopicViewModel> MainTopics { get; set; } = [];
}
