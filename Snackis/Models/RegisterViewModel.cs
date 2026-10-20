using System.ComponentModel.DataAnnotations;

namespace Snackis.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Ange användarnamn.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Användarnamnet måste vara mellan {2} och {1} tecken.")]
    [Display(Name = "Användarnamn")]
    public string UserName { get; set; } = "";

    [Required(ErrorMessage = "Ange e-postadress.")]
    [EmailAddress(ErrorMessage = "Ange en giltig e-postadress.")]
    [Display(Name = "E-post")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Ange lösenord.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Lösenordet måste vara minst {2} tecken.")]
    [DataType(DataType.Password)]
    [Display(Name = "Lösenord")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Upprepa lösenordet.")]
    [Compare(nameof(Password), ErrorMessage = "Lösenorden matchar inte.")]
    [DataType(DataType.Password)]
    [Display(Name = "Upprepa lösenord")]
    public string ConfirmPassword { get; set; } = "";
}
