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
    public class PaginationRequestDtoValidator : AbstractValidator<PaginationRequestDto>
    {
        public PaginationRequestDtoValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.FieldMustBeGreaterThanZeroError);

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.FieldMustBeGreaterThanZeroError);
        }
    }
}
