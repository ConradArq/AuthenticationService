using AuthenticationService.Application.Interfaces.Strategies.Delete;
using AuthenticationService.Application.Strategies.Delete.Enums;
using AuthenticationService.Domain.Enums;
using AuthenticationService.Domain.Interfaces.Models;
using AuthenticationService.Domain.Interfaces.Repositories;

namespace AuthenticationService.Application.Strategies.Delete
{
    /// <summary>
    /// A deletion strategy that performs a soft delete by setting the status of the entity and its related children to inactive.
    /// </summary>
    /// <typeparam name="T">The type of the entity to soft delete, which must implement <see cref="IBaseDomainModel"/>.</typeparam>
    public class SoftDeleteStrategy<T> : IDeletionStrategy<T> where T : class, IBaseDomainModel
    {
        public DeletionMode DeletionMode => DeletionMode.Soft;

        public void Delete(T entity, IUnitOfWork unitOfWork)
        {
            if (entity is IBaseDomainModel baseEntity)
            {
                baseEntity.StatusId = (int)Status.Inactive;
            }

            var collections = typeof(T).GetProperties()
                .Where(p =>
                    typeof(IEnumerable<IBaseDomainModel>).IsAssignableFrom(p.PropertyType) &&
                    p.PropertyType != typeof(string));

            foreach (var prop in collections)
            {
                if (prop.GetValue(entity) is IEnumerable<IBaseDomainModel> children)
                {
                    foreach (var child in children)
                        child.StatusId = (int)Status.Inactive;
                }
            }
        }      
    }
}
