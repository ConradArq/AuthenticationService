using FluentValidation;
using AuthenticationService.Shared.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.ApplicationUser.Commands.Update
{
    public class UpdateApplicationUserCommandValidator : AbstractValidator<UpdateApplicationUserCommand>
    {
        public UpdateApplicationUserCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .MaximumLength(100).WithMessage(string.Format(ValidationMessages.FieldMaxLengthError, 100));

            RuleFor(x => x.LastName)
                .MaximumLength(100).WithMessage(string.Format(ValidationMessages.FieldMaxLengthError, 100));

            RuleFor(x => x.UserName)
                .MaximumLength(50).WithMessage(string.Format(ValidationMessages.FieldMaxLengthError, 50))
                .When(x => !string.IsNullOrWhiteSpace(x.UserName));

            RuleFor(x => x.RoleNames)
                .ForEach(role => role
                    .Must(roleName => !string.IsNullOrWhiteSpace(roleName)).WithMessage(ValidationMessages.ListCannotContainEmptyOrNullItemsError))
                .When(x => x.RoleNames != null && x.RoleNames.Any());

            RuleFor(x => x.StatusId)
                .Must(value => value == null || Enum.IsDefined(typeof(Domain.Enums.Status), value))
                .WithMessage(x => string.Format(
                    ValidationMessages.InvalidEnumError,
                    nameof(Domain.Enums.Status),
                    string.Join(", ", Enum.GetNames(typeof(Domain.Enums.Status)))));
        }
    }
}
