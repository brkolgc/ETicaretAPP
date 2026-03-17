using MediatR;
using System.Reflection;

namespace ETicaretAPI.Application.Features.Queries.Authorization
{
    public class GetAuthorizeDefinitionEndpointsQueryRequest : IRequest<List<GetAuthorizeDefinitionEndpointsQueryResponse>>
    {
        public Assembly Assembly { get; set; }
    }
}