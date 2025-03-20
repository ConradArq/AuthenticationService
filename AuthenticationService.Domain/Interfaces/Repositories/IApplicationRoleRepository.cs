using AuthenticationService.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Domain.Interfaces.Repositories
{
    public interface IApplicationRoleRepository : IGenericRepository<ApplicationRole>
    {
        Task<bool> RoleExistsAsync(string roleName);
        Task<ApplicationRole?> GetByIdAsync(string roleId);
        Task<(bool IsSuccess, List<string> Errors)> CreateRoleAsync(ApplicationRole role);
        Task<(bool IsSuccess, List<string> Errors)> UpdateRoleAsync(ApplicationRole role);
        Task<(bool IsSuccess, List<string> Errors)> DeleteRoleAsync(ApplicationRole role);
    }
}
