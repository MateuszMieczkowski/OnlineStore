using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Client.Models.Accounts;

public class ChangePasswordModel
{
    [Required(ErrorMessage = "Obecne hasło musi być wypełnione")]
    public string CurrentPassword { get; set; } = "";

    [Required(ErrorMessage = "Nowe hasło musi być wypełnione")]
    [MinLength(12, ErrorMessage = "Nowe hasło musi mieć co najmniej 12 znaków.")]
    [MaxLength(18, ErrorMessage = "Nowe hasło może mieć maksymalnie 18 znaków.")]
    public string NewPassword { get; set; } = "";

    [Required(ErrorMessage = "Nowe hasło musi być potwierdzone")]
    [Compare("NewPassword", ErrorMessage = "Nowa hasła muszą się zgadzać")]
    public string ConfirmNewPassword { get; set; } = "";
}