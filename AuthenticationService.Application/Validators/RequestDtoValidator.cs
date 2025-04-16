using AuthenticationService.Shared.Dtos;
using AuthenticationService.Shared.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Validators
{
    public class RequestDtoValidator : AbstractValidator<RequestDto>
    {
        public RequestDtoValidator()
        {
            RuleFor(x => x.OrderDirection)
                .Must(direction => string.IsNullOrWhiteSpace(direction) || direction is "asc" or "desc")
                .WithMessage(ValidationMessages.InvalidOrderDirection);

            RuleFor(x => x.OrderDirection)
                .Empty()
                .When(x => string.IsNullOrWhiteSpace(x.OrderBy))
                .WithMessage(ValidationMessages.OrderDirectionWithoutOrderByError);
        }
    }
}
