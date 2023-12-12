using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Client.Models.Accounts;

public class ResetPasswordModel
{
    [Required(ErrorMessage = "Nowe hasło musi być wypełnione")]
    public string NewPassword { get; set; } = "";

    [Required(ErrorMessage = "Nowe hasło musi być potwierdzone")]
    [Compare("NewPassword", ErrorMessage = "Nowa hasła muszą się zgadzać")]
    public string ConfirmNewPassword { get; set; } = "";
}
