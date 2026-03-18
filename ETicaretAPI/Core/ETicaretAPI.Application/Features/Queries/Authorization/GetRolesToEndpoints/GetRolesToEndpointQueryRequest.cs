using MediatR;

namespace ETicaretAPI.Application.Features.Queries.Authorization.GetRolesToEndpoints
{
    public class GetRolesToEndpointQueryRequest : IRequest<GetRolesToEndpointQueryResponse>
    {
        public string Code { get; set; }
        public string Menu { get; set; }
    }
}