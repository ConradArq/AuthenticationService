using FluentValidation;
using AuthenticationService.Shared.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.ApplicationMenu.Commands.Update
{
    public class UpdateApplicationMenuCommandValidator : AbstractValidator<UpdateApplicationMenuCommand>
    {
        public UpdateApplicationMenuCommandValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(100).WithMessage(string.Format(ValidationMessages.FieldMaxLengthError, 100))
                .When(x => !string.IsNullOrWhiteSpace(x.Title));

            RuleFor(x => x.Path)
                .MaximumLength(200).WithMessage(string.Format(ValidationMessages.FieldMaxLengthError, 200))
                .When(x => !string.IsNullOrWhiteSpace(x.Path));

            RuleFor(x => x.IconType)
                .MaximumLength(50).WithMessage(string.Format(ValidationMessages.FieldMaxLengthError, 50))
                .When(x => !string.IsNullOrWhiteSpace(x.IconType));

            RuleFor(x => x.Icon)
                .MaximumLength(50).WithMessage(string.Format(ValidationMessages.FieldMaxLengthError, 50))
                .When(x => !string.IsNullOrWhiteSpace(x.Icon));

            RuleFor(x => x.Class)
                .MaximumLength(100).WithMessage(string.Format(ValidationMessages.FieldMaxLengthError, 100))
                .When(x => !string.IsNullOrWhiteSpace(x.Class));

            RuleFor(x => x.GroupTitle)
                .Must(value => value != null).WithMessage(ValidationMessages.RequiredFieldError)
                .When(x => x.GroupTitle.HasValue);

            RuleFor(x => x.Badge)
                .MaximumLength(50).WithMessage(string.Format(ValidationMessages.FieldMaxLengthError, 50))
                .When(x => !string.IsNullOrWhiteSpace(x.Badge));

            RuleFor(x => x.BadgeClass)
                .MaximumLength(100).WithMessage(string.Format(ValidationMessages.FieldMaxLengthError, 100))
                .When(x => !string.IsNullOrWhiteSpace(x.BadgeClass));

            RuleFor(x => x.Order)
                .GreaterThan(0).WithMessage(ValidationMessages.FieldMustBeGreaterThanZeroError);

            RuleFor(x => x.ParentApplicationMenuId)
                .GreaterThan(0).WithMessage(ValidationMessages.FieldMustBeGreaterThanZeroError)
                .When(x => x.ParentApplicationMenuId.HasValue);

            RuleFor(x => x.StatusId)
                .Must(value => Enum.IsDefined(typeof(Domain.Enums.Status), value))
                .WithMessage(x => string.Format(
                    ValidationMessages.InvalidEnumError,
                    nameof(Domain.Enums.Status),
                    string.Join(", ", Enum.GetNames(typeof(Domain.Enums.Status))))
                );
        }
    }
}
