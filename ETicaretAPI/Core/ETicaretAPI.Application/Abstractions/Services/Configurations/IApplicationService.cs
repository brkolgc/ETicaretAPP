using ETicaretAPI.Application.DTOs.Configuration;
using System.Reflection;

namespace ETicaretAPI.Application.Abstractions.Services.Configurations
{
    public interface IApplicationService
    {
        List<Menu> GetAuthorizeDefinitionEndpoints(Assembly assembly);
    }
}
