using AuthenticationService.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Application.Attributes
{
    /// <summary>
    /// A custom validation attribute to ensure that the value of a property matches a valid value in a specified enum type.
    /// </summary>
    /// <remarks>
    /// This attribute checks if the value is either null or a defined value in the specified enum.
    /// If the value is invalid, an error message indicating the allowed enum values is returned.
    /// </remarks>
    public class EnumValueValidationAttribute : ValidationAttribute
    {
        private readonly Type _enumType;

        public EnumValueValidationAttribute(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Provided type must be an enum.", nameof(enumType));
            }

            _enumType = enumType;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || Enum.IsDefined(_enumType, value))
            {
                return ValidationResult.Success;
            }

            var validValues = string.Join(", ", Enum.GetNames(_enumType));

            var errorMessage = string.Format(
                ValidationMessages.InvalidEnumError,
                _enumType.Name,
                validValues
            );

            return new ValidationResult(errorMessage);
        }
    }

}
