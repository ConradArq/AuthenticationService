using AuthenticationService.Domain.Interfaces.Repositories;
using AuthenticationService.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.Infrastructure.Persistence.Repositories
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserRepository(AuthenticationServiceDbContext context, UserManager<ApplicationUser> userManager)
            : base(context)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser?> GetByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<List<string>> GetRolesAsync(ApplicationUser user)
        {
            return (await _userManager.GetRolesAsync(user)).ToList();
        }

        public async Task<(bool IsSuccess, List<string> Errors)> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return (result.Succeeded, result.Errors.Select(e => e.Description).ToList());
        }

        public async Task<(bool IsSuccess, List<string> Errors)> UpdateUserAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            return (result.Succeeded, result.Errors.Select(e => e.Description).ToList());
        }

        public async Task<(bool IsSuccess, List<string> Errors)> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);
            return (result.Succeeded, result.Errors.Select(e => e.Description).ToList());
        }

        public async Task<(bool IsSuccess, List<string> Errors)> AssignRolesAsync(ApplicationUser user, List<string> roles)
        {
            var result = await _userManager.AddToRolesAsync(user, roles);
            return (result.Succeeded, result.Errors.Select(e => e.Description).ToList());
        }

        public async Task<(bool IsSuccess, List<string> Errors)> RemoveRolesAsync(ApplicationUser user, List<string> roles)
        {
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            return (result.Succeeded, result.Errors.Select(e => e.Description).ToList());
        }
    }
}
