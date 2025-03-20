using AuthenticationService.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Application.Attributes
{
    /// <summary>
    /// A custom validation attribute to ensure a field's value is greater than zero, with support for localized error messages.
    /// This attribute is useful for fields like IDs, page numbers, or counts where the value must be positive.
    /// It overcomes the limitation that .NET validation attributes embed error messages into the attribute 
    /// assembly at compile time, preventing runtime localization.
    /// </summary>
    public class GreaterThanZeroAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int intValue && intValue <= 0)
            {
                return new ValidationResult(ValidationMessages.FieldMustBeGreaterThanZeroError);
            }

            return ValidationResult.Success;
        }
    }

}
