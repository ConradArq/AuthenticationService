using FluentValidation;
using AuthenticationService.Shared.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.Authentication.Commands.LoginVerifyOtp
{
    public class LoginVerifyOtpCommandValidator : AbstractValidator<LoginVerifyOtpCommand>
    {
        public LoginVerifyOtpCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(ValidationMessages.RequiredFieldError);

            RuleFor(x => x.Otp)
                .NotEmpty().WithMessage(ValidationMessages.RequiredFieldError);
        }
    }
}
