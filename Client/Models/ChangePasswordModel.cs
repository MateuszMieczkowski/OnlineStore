using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Client.Models;

public class ChangePasswordModel
{
    [Required(ErrorMessage = "Obecne hasło musi być wypełnione")]
    public string CurrentPassword { get; set; } = "";

    [Required(ErrorMessage = "Nowe hasło musi być wypełnione")]
    public string NewPassword { get; set; } = "";

    [Required(ErrorMessage = "Nowe hasło musi być potwierdzone")]
    [Compare("NewPassword", ErrorMessage = "Nowa hasła muszą się zgadzać")]
    public string ConfirmNewPassword { get; set; } = "";
}