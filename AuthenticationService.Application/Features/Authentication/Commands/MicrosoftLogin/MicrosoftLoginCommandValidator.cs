using FluentValidation;
using AuthenticationService.Shared.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.Authentication.Commands.MicrosoftLogin
{
    public class MicrosoftLoginCommandValidator : AbstractValidator<MicrosoftLoginCommand>
    {
        public MicrosoftLoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(ValidationMessages.RequiredFieldError)
                .EmailAddress().WithMessage(ValidationMessages.InvalidEmailError);
        }
    }
}
