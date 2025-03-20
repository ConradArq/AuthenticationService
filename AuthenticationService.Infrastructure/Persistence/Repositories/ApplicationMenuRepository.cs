using AuthenticationService.Domain.Models.Entities;
using AuthenticationService.Domain.Interfaces.Repositories;

namespace AuthenticationService.Infrastructure.Persistence.Repositories
{
    public class ApplicationMenuRepository : GenericRepository<ApplicationMenu>, IApplicationMenuRepository
    {
        public ApplicationMenuRepository(AuthenticationServiceDbContext context) : base(context)
        {
        }
    }
}
