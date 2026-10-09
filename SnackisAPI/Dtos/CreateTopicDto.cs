using System.ComponentModel.DataAnnotations;
using SnackisAPI.Helpers;

namespace SnackisAPI.Dtos;

public class CreateTopicDto
{
    [Required, StringLength(FieldLengths.TopicName, MinimumLength = 2)]
    public string Name { get; set; } = "";

    public Guid? ParentId { get; set; }
}
