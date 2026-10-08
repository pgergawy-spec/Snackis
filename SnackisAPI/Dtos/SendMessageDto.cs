using System.ComponentModel.DataAnnotations;
using SnackisAPI.Helpers;

namespace SnackisAPI.Dtos;

public class SendMessageDto
{
    [Required]
    public string ReceiverUserName { get; set; } = "";

    [Required, StringLength(FieldLengths.MessageContent, MinimumLength = 1)]
    public string Content { get; set; } = "";
}
