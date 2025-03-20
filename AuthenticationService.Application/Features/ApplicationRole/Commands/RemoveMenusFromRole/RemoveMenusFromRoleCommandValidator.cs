using FluentValidation;
using AuthenticationService.Shared.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.ApplicationRole.Commands.RemoveMenusFromRole
{
    public class RemoveMenusFromRoleCommandValidator : AbstractValidator<RemoveMenusFromRoleCommand>
    {
        public RemoveMenusFromRoleCommandValidator()
        {
            RuleFor(x => x.ApplicationRoleId)
                .NotEmpty().WithMessage(ValidationMessages.RequiredFieldError)
                .MaximumLength(100).WithMessage(string.Format(ValidationMessages.FieldMaxLengthError, 100));

            RuleFor(x => x.ApplicationMenuIds)
                .NotEmpty().WithMessage(ValidationMessages.RequiredFieldError)
                .ForEach(menuId => menuId.GreaterThan(0).WithMessage(ValidationMessages.ListItemsMustBeGreaterThanZeroError));
        }
    }
}
