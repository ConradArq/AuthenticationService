using FluentValidation;
using AuthenticationService.Shared.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.Authentication.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(ValidationMessages.RequiredFieldError)
                .EmailAddress().WithMessage(ValidationMessages.InvalidEmailError);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ValidationMessages.RequiredFieldError);
        }
    }
}
