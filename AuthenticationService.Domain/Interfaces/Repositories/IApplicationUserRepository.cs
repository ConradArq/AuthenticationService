using AuthenticationService.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Domain.Interfaces.Repositories
{
    public interface IApplicationUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<ApplicationUser?> GetByIdAsync(string id);
        Task<ApplicationUser?> GetByEmailAsync(string email);
        Task<ApplicationUser?> GetByUserNameAsync(string userName);
        Task<List<string>> GetRolesAsync(ApplicationUser user);
        Task<(bool IsSuccess, List<string> Errors)> CreateUserAsync(ApplicationUser user, string password);
        Task<(bool IsSuccess, List<string> Errors)> UpdateUserAsync(ApplicationUser user);
        Task<(bool IsSuccess, List<string> Errors)> DeleteUserAsync(ApplicationUser user);
        Task<(bool IsSuccess, List<string> Errors)> AssignRolesAsync(ApplicationUser user, List<string> roles);
        Task<(bool IsSuccess, List<string> Errors)> RemoveRolesAsync(ApplicationUser user, List<string> roles);
    }
}
