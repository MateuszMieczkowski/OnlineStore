using FluentValidation;
using OnlineStore.Shared.Models;

namespace OnlineStore.Server.Validators;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator(OnlineStoreDbContext dbContext)
    {
        RuleFor(x => x.Login)
            .NotEmpty();
        RuleFor(x => x.Password)
            .MinimumLength(6);

        RuleFor(x => x.ConfirmPassword)
            .Equal(e => e.Password);

        RuleFor(x => x.Login)
            .Custom((value, context) =>
            {
                var emailInUse = dbContext.Users.Any(u => u.Email == value);
                if (emailInUse) context.AddFailure("Email", "That login is taken");
            });
    }
}