using FluentValidation;
using AuthenticationService.Shared.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.Identity.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty();
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage(ValidationMessages.RequiredFieldError)
                .MaximumLength(50).WithMessage(string.Format(ValidationMessages.FieldMaxLengthError, 50))
                .Must(password => IsValidPassword(password)).WithMessage(ValidationMessages.InvalidPasswordFormat);
        }

        private bool IsValidPassword(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            return password.Length >= 8 &&
                   password.Length <= 50 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit) &&
                   password.Any(ch => !char.IsLetterOrDigit(ch));
        }
    }
}
