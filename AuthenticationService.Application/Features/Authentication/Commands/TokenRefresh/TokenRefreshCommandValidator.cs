using FluentValidation;
using AuthenticationService.Shared.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Features.Authentication.Commands.TokenRefresh
{
    public class TokenRefreshCommandValidator : AbstractValidator<TokenRefreshCommand>
    {
        public TokenRefreshCommandValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage(ValidationMessages.RequiredFieldError);
        }
    }
}
