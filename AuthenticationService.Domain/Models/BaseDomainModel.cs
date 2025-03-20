using AuthenticationService.Domain.Interfaces.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace AuthenticationService.Domain.Models
{
    public abstract class BaseDomainModel: IBaseDomainModel
    {
        private static readonly Dictionary<string, PropertyInfo> PropertyCache = new Dictionary<string, PropertyInfo>();

        // Explicitly implemented property to fulfill the contract of the IBaseDomainModel interface. It won't be mapped by EF
        // because explicit interface implementations are ignored during database schema generation.
        object IBaseDomainModel.Id
        {
            get => Id;
            set
            {
                if (value is int intValue)
                    Id = intValue;
                else
                    throw new InvalidOperationException("Id must be of type int.");
            }
        }

        // The public property Id ensures that the property is part of the class's public API. This avoids runtime errors when accessing
        // Id in LINQ queries, as explicit interface implementations (like IBaseDomainModel.Id) are not directly accessible.
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; }

        public int StatusId { get; set; } = (int)Enums.Status.Active;

        // Validates property based on data annotations and caches properties to speed up reflection
        protected void ValidateProperty(string propertyName)
        {
            if (!PropertyCache.TryGetValue(propertyName, out var property))
            {
                property = GetType().GetProperty(propertyName);

                if (property == null)
                {
                    throw new ValidationException($"Property '{propertyName}' does not exist.");
                }

                PropertyCache[propertyName] = property;
            }

            var validationContext = new ValidationContext(this) { MemberName = propertyName };
            Validator.ValidateProperty(property.GetValue(this), validationContext);
        }
    }
}
