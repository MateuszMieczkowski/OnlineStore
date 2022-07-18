namespace SneakersBase.Shared.Models
{
    public class RegisterUserDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int RoleId { get; set; } = 1;
    }
}
