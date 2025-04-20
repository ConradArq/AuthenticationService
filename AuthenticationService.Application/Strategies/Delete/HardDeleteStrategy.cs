using AuthenticationService.Application.Interfaces.Strategies.Delete;
using AuthenticationService.Application.Strategies.Delete.Enums;
using AuthenticationService.Domain.Interfaces.Models;
using AuthenticationService.Domain.Interfaces.Repositories;

namespace AuthenticationService.Application.Strategies
{
    /// <summary>
    /// A deletion strategy that performs a hard delete by removing the entity from the database context.
    /// </summary>
    /// <typeparam name="T">The type of the entity to delete.</typeparam>
    public class HardDeleteStrategy<T> : IDeletionStrategy<T> where T : class, IBaseDomainModel
    {
        public DeletionMode DeletionMode => DeletionMode.Hard;

        public void Delete(T entity, IUnitOfWork unitOfWork)
        {
            unitOfWork.Repository<T>().Delete(entity);
        }
    }
}
