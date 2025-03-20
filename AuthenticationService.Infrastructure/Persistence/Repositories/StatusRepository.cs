using AuthenticationService.Domain.Interfaces.Repositories;
using AuthenticationService.Domain.Models.Entities;

namespace AuthenticationService.Infrastructure.Persistence.Repositories
{
    public class StatusRepository : GenericRepository<Status>, IStatusRepository
    {
        public StatusRepository(AuthenticationServiceDbContext context) : base(context)
        {
        }
    }
}
