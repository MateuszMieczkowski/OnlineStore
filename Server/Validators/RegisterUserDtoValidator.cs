using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SneakersBase.Shared.Models;

namespace SneakersBase.Server.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(SneakersDbContext dbContext)
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
                    var emailInUse = dbContext.Users.Any(u => u.Login == value);
                    if (emailInUse)
                    {
                        context.AddFailure("Login", "That login is taken");
                    }
                });
        }
    }
}
