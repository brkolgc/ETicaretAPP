namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IRoleService
    {
        Task<(IDictionary<string, string>,int TotalRoleCount)> GetAllRolesAsync(int page, int size);
        Task<(string id, string name)> GetRoleByIdAsync(string id);

        Task<bool> CreateRoleAsync(string name);
        Task<bool> DeleteRoleAsync(string id);
        Task<bool> UpdateRoleAsync(string id, string name);
    }
}
