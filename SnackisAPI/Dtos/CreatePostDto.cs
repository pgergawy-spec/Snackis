using System.ComponentModel.DataAnnotations;
using SnackisAPI.Helpers;

namespace SnackisAPI.Dtos;

public class CreatePostDto
{
    [Required, StringLength(FieldLengths.PostTitle, MinimumLength = 2)]
    public string Title { get; set; } = "";

    [Required, StringLength(FieldLengths.PostContent, MinimumLength = 2)]
    public string Content { get; set; } = "";

    public Guid TopicId { get; set; }
}
