using System.ComponentModel.DataAnnotations;
using Snackis.Helpers;

namespace Snackis.Models;

public class SendMessageViewModel
{
    [Required(ErrorMessage = "Ange vem meddelandet ska till.")]
    [Display(Name = "Till (användarnamn)")]
    public string ReceiverUserName { get; set; } = "";

    [Required(ErrorMessage = "Skriv ett meddelande.")]
    [StringLength(FieldLengths.MessageContent, MinimumLength = 1, ErrorMessage = "Meddelandet får vara högst {1} tecken.")]
    [Display(Name = "Meddelande")]
    public string Content { get; set; } = "";
}
