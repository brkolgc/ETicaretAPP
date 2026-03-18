using ETicaretAPI.Application.DTOs.Configuration;

namespace ETicaretAPI.Application.Features.Queries.Authorization.GetAuthorizeDefinitionEndpoints
{
    public class GetAuthorizeDefinitionEndpointsQueryResponse
    {
        public string Name { get; set; }
        public List<DTOs.Configuration.Action> Actions { get; set; } = new();
    }
}