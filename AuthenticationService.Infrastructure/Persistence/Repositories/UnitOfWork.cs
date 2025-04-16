using AuthenticationService.Domain.Interfaces.Models;
using AuthenticationService.Domain.Interfaces.Repositories;

namespace AuthenticationService.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private Dictionary<string, object> _repositories = new();
        private readonly AuthenticationServiceDbContext _context;

        private IApplicationMenuRepository _applicationMenuRepository;
        private IApplicationUserRepository _applicationUserRepository;
        private IApplicationRoleRepository _applicationRoleRepository;
        private IApplicationRoleMenuRepository _applicationRoleMenuRepository;
        private IStatusRepository _statusRepository;
        private IRefreshTokenRepository _refreshTokenRepository;

        public UnitOfWork(AuthenticationServiceDbContext context, IApplicationMenuRepository applicationMenuRepository, 
            IApplicationUserRepository applicationUserRepository, IApplicationRoleRepository applicationRoleRepository, 
            IApplicationRoleMenuRepository applicationRoleMenuRepository, IStatusRepository statusRepository, 
            IRefreshTokenRepository refreshTokenRepository)
        {
            _context = context;
            _applicationMenuRepository = applicationMenuRepository;
            _applicationUserRepository = applicationUserRepository;
            _applicationRoleRepository = applicationRoleRepository;
            _applicationRoleMenuRepository = applicationRoleMenuRepository;
            _statusRepository = statusRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public IApplicationMenuRepository ApplicationMenuRepository => _applicationMenuRepository;
        public IApplicationUserRepository ApplicationUserRepository => _applicationUserRepository;
        public IApplicationRoleRepository ApplicationRoleRepository => _applicationRoleRepository;
        public IApplicationRoleMenuRepository ApplicationRoleMenuRepository => _applicationRoleMenuRepository;
        public IStatusRepository StatusRepository => _statusRepository;
        public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository;

        /// <summary>
        /// Centralizes save logic, ensuring that all changes to the context are persisted at once.
        /// </summary>
        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

        /// <summary>
        /// Executes a transaction, useful when you need to perform multiple operations that require intermediate saves or interdependent 
        /// actions. For example, when saving data to one entity, using its value for further operations, and then saving another entity.
        /// This also ensures atomicity when performing actions such as saving to the database and 
        /// sending an email, maintaining consistency across multiple database actions and external processes.
        /// </summary>
        public async Task CompleteTransactionAsync(Func<Task> functionTransaction)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await functionTransaction();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Provides a generic repository instance for the specified TEntity type which simplifies access to basic CRUD operations 
        /// for any entity type, allowing developers to manage multiple entities without writing repetitive code for each repository.
        /// </summary>
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class, IBaseDomainModel
        {
            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = new GenericRepository<TEntity>(_context);
                _repositories[type] = repositoryInstance;
            }

            return (IGenericRepository<TEntity>)_repositories[type];
        }

        public object GetRepository(Type entityType)
        {
            if (entityType == null)
                throw new ArgumentNullException(nameof(entityType));

            if (!typeof(IBaseDomainModel).IsAssignableFrom(entityType))
                throw new InvalidOperationException($"The type {entityType.Name} is not a valid domain model.");

            var typeName = entityType.Name;

            if (!_repositories.ContainsKey(typeName))
            {
                var repositoryType = typeof(GenericRepository<>).MakeGenericType(entityType);
                var repositoryInstance = Activator.CreateInstance(repositoryType, _context);

                if (repositoryInstance == null)
                    throw new InvalidOperationException($"Could not create repository instance for type {entityType.Name}.");

                _repositories[typeName] = repositoryInstance;
            }

            return _repositories[typeName];
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
