using AuthenticationService.Shared.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Attributes
{
    /// <summary>
    /// A custom validation attribute for string length that supports localization.
    /// This attribute extends the built-in <see cref="StringLengthAttribute"/> to support resource-based error messages.
    /// It addresses the limitation that .NET's <see cref="StringLengthAttribute"/> embeds error messages 
    /// into the attribute assembly at compile time, preventing localization with resource files.
    /// </summary>
    public class LocalizedStringLengthAttribute : ValidationAttribute
    {
        private readonly StringLengthAttribute _stringLengthAttribute;

        public LocalizedStringLengthAttribute(int maximumLength)
        {
            _stringLengthAttribute = new StringLengthAttribute(maximumLength);
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (!_stringLengthAttribute.IsValid(value))
            {
                var errorMessage = string.Format(ValidationMessages.FieldMaxLengthError, _stringLengthAttribute.MaximumLength);
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
