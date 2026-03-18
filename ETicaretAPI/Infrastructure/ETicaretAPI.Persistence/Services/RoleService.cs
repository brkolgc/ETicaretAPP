using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Persistence.Services
{
    public class RoleService : IRoleService
    {
        readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> CreateRoleAsync(string name)
        {
            IdentityResult result = await _roleManager.CreateAsync(new() { Id = Guid.NewGuid().ToString(), Name = name });
            return result.Succeeded;
        }

        public async Task<bool> DeleteRoleAsync(string id)
        {
            IdentityResult result = await _roleManager.DeleteAsync(new() { Id = id });
            return result.Succeeded;
        }

        public async Task<(IDictionary<string, string>, int TotalRoleCount)> GetAllRolesAsync(int page, int size)
        {
            var query = _roleManager.Roles;
            int totalCount = await query.CountAsync();
            IDictionary<string, string?> roles;

            if (page == -1 || size == -1)
                roles = await query.ToDictionaryAsync(role => role.Id, role => role.Name);
            else
                roles = await query
                        .Skip(page * size)
                        .Take(size)
                        .ToDictionaryAsync(role => role.Id, role => role.Name);

            return (roles, totalCount);
        }

        public async Task<(string id, string name)> GetRoleByIdAsync(string id)
        {
            string role = await _roleManager.GetRoleIdAsync(new() { Id = id });
            return (id, role);
        }

        public async Task<bool> UpdateRoleAsync(string id, string name)
        {
            IdentityResult result = await _roleManager.UpdateAsync(new() { Id = id, Name = name });
            return result.Succeeded;
        }
    }
}
