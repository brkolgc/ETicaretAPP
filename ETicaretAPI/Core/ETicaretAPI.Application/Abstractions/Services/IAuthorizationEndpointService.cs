using System.Reflection;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IAuthorizationEndpointService
    {
        public Task AssignRoleEndpointAsync(string[] roles,string menu, string code, Assembly assembly);
        public Task<List<string>> GetRolesToEndpointAsync(string code,string menu);
    }
}
