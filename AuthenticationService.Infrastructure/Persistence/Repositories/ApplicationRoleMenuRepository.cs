using AuthenticationService.Domain.Models.Entities;

namespace AuthenticationService.Infrastructure.Persistence.Repositories
{
    public class ApplicationRoleMenuRepository : GenericRepository<ApplicationRoleMenu>, IApplicationRoleMenuRepository
    {
        public ApplicationRoleMenuRepository(AuthenticationServiceDbContext context) : base(context)
        {
        }
    }
}
