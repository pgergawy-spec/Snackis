using System.ComponentModel.DataAnnotations;

namespace Snackis.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Ange användarnamn.")]
    [Display(Name = "Användarnamn")]
    public string UserName { get; set; } = "";

    [Required(ErrorMessage = "Ange lösenord.")]
    [DataType(DataType.Password)]
    [Display(Name = "Lösenord")]
    public string Password { get; set; } = "";
}
