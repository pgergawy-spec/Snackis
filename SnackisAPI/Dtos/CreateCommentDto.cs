using System.ComponentModel.DataAnnotations;
using SnackisAPI.Helpers;

namespace SnackisAPI.Dtos;

public class CreateCommentDto
{
    public Guid PostId { get; set; }

    [Required, StringLength(FieldLengths.CommentContent, MinimumLength = 1)]
    public string Content { get; set; } = "";
}
