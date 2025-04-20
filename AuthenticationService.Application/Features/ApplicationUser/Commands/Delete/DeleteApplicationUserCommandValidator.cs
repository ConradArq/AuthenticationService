using AuthenticationService.Application.Strategies.Delete.Enums;
using AuthenticationService.Shared.Resources;
using FluentValidation;

namespace AuthenticationService.Application.Features.ApplicationUser.Commands.Delete
{
    public class DeleteApplicationUserCommandValidator:AbstractValidator<DeleteApplicationUserCommand>
    {
        public DeleteApplicationUserCommandValidator() 
        {
            RuleFor(x => x.DeletionMode)
                .Must(value => Enum.IsDefined(typeof(DeletionMode), value))
                .WithMessage(x => string.Format(
                    ValidationMessages.InvalidEnumError,
                    nameof(DeletionMode),
                    string.Join(", ", Enum.GetNames(typeof(DeletionMode)))));
        }
    }
}
