using AuthenticationService.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Application.Attributes
{
    /// <summary>
    /// A custom validation attribute for required fields that supports localization.
    /// This attribute extends the built-in <see cref="RequiredAttribute"/> to support resource-based error messages.
    /// This attribute is used because .NET's built-in <see cref="RequiredAttribute"/> embeds error messages directly into 
    /// the attribute assembly at compile time, which prevents the use of resource-based messages that are resolved at runtime.
    /// </summary>
    public class LocalizedRequiredAttribute : ValidationAttribute
    {
        private readonly RequiredAttribute _requiredAttribute;

        public LocalizedRequiredAttribute()
        {
            _requiredAttribute = new RequiredAttribute();
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            return _requiredAttribute.IsValid(value)
                ? ValidationResult.Success
                : new ValidationResult(ValidationMessages.RequiredFieldError);
        }
    }

}
