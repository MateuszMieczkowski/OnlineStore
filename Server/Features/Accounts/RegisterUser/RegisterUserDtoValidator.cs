using FluentValidation;
using OnlineStore.Shared.Accounts;

namespace OnlineStore.Server.Features.Accounts.RegisterUser;

public class RegisterUserDtoValidator : AbstractValidator<RegisterAdmin>
{
    public RegisterUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty();
        RuleFor(x => x.Password)
            .MinimumLength(12).WithMessage("Hasło musi mieć co najmniej 12 znaków.")
            .MaximumLength(18).WithMessage("Hasło może mieć maksymalnie 18 znaków.");

        RuleFor(x => x.ConfirmPassword)
            .Equal(e => e.Password);
    }
}