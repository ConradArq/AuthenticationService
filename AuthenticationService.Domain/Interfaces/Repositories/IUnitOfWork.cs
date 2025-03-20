using AuthenticationService.Domain.Models;
using AuthenticationService.Infrastructure.Persistence.Repositories;

namespace AuthenticationService.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IApplicationUserRepository ApplicationUserRepository { get; }
        IApplicationMenuRepository ApplicationMenuRepository { get; }
        IApplicationRoleRepository ApplicationRoleRepository { get; }
        IApplicationRoleMenuRepository ApplicationRoleMenuRepository { get; }
        IStatusRepository StatusRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }

        Task CompleteTransactionAsync(Func<Task> functionTransaction);
        void Dispose();
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseDomainModel;
        object GetRepository(Type entityType);
        Task<int> SaveAsync();
    }
}