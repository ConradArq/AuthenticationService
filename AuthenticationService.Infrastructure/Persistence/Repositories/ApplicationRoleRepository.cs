using AuthenticationService.Domain.Interfaces.Repositories;
using AuthenticationService.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.Infrastructure.Persistence.Repositories
{
    public class ApplicationRoleRepository : GenericRepository<ApplicationRole>, IApplicationRoleRepository
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ApplicationRoleRepository(AuthenticationServiceDbContext context, RoleManager<ApplicationRole> roleManager)
            : base(context)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<ApplicationRole?> GetByIdAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }

        public async Task<(bool IsSuccess, List<string> Errors)> CreateRoleAsync(ApplicationRole role)
        {
            var result = await _roleManager.CreateAsync(role);
            return (result.Succeeded, result.Errors.Select(e => e.Description).ToList());
        }

        public async Task<(bool IsSuccess, List<string> Errors)> UpdateRoleAsync(ApplicationRole role)
        {
            var result = await _roleManager.UpdateAsync(role);
            return (result.Succeeded, result.Errors.Select(e => e.Description).ToList());
        }

        public async Task<(bool IsSuccess, List<string> Errors)> DeleteRoleAsync(ApplicationRole role)
        {
            var result = await _roleManager.DeleteAsync(role);
            return (result.Succeeded, result.Errors.Select(e => e.Description).ToList());
        }
    }
}
