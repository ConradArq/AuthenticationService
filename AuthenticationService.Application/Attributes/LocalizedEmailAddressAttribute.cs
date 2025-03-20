using AuthenticationService.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Application.Attributes
{
    /// <summary>
    /// A custom validation attribute for validating email addresses with support for localized error messages.
    /// This attribute leverages the built-in <see cref="EmailAddressAttribute"/> for validation logic
    /// but provides localized error messages by referencing a resource file.
    /// It addresses the limitation of .NET's <see cref="EmailAddressAttribute"/>, which embeds error 
    /// messages into the attribute assembly and does not support runtime localization.
    /// </summary>
    public class LocalizedEmailAddressAttribute : ValidationAttribute
    {
        private readonly EmailAddressAttribute _emailAddressAttribute;

        public LocalizedEmailAddressAttribute()
        {
            _emailAddressAttribute = new EmailAddressAttribute();
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            return _emailAddressAttribute.IsValid(value)
                ? ValidationResult.Success
                : new ValidationResult(ValidationMessages.InvalidEmailError);
        }
    }

}
