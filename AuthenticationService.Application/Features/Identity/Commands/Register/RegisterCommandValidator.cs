using FluentValidation;
using AuthenticationService.Shared.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.Identity.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage(ValidationMessages.RequiredFieldError)
                .MaximumLength(100).WithMessage(string.Format(ValidationMessages.FieldMaxLengthError, 100));

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(ValidationMessages.RequiredFieldError)
                .MaximumLength(100).WithMessage(string.Format(ValidationMessages.FieldMaxLengthError, 100));

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(ValidationMessages.RequiredFieldError)
                .EmailAddress().WithMessage(ValidationMessages.InvalidEmailError);

            RuleFor(x => x.UserName)
                .MaximumLength(50).WithMessage(string.Format(ValidationMessages.FieldMaxLengthError, 50));

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ValidationMessages.RequiredFieldError)
                .MaximumLength(50).WithMessage(string.Format(ValidationMessages.FieldMaxLengthError, 50))
                .Must(password => IsValidPassword(password)).WithMessage(ValidationMessages.InvalidPasswordFormat);

            RuleFor(x => x.RoleNames)
                .NotEmpty().WithMessage(ValidationMessages.RequiredFieldError)
                .ForEach(role => role
                    .Must(roleName => !string.IsNullOrWhiteSpace(roleName)).WithMessage(ValidationMessages.ListCannotContainEmptyOrNullItemsError));

            RuleFor(x => x.StatusId)
                .Must(value => Enum.IsDefined(typeof(Domain.Enums.Status), value))
                .WithMessage(x => string.Format(
                    ValidationMessages.InvalidEnumError,
                    nameof(Domain.Enums.Status),
                    string.Join(", ", Enum.GetNames(typeof(Domain.Enums.Status)))));
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
