using MediatR;
using System.Reflection;

namespace ETicaretAPI.Application.Features.Commands.Authorization.AssignRoleEndpoint
{
    public class AssignRoleEndpointCommandRequest : IRequest<AssignRoleEndpointCommandResponse>
    {
        public string[] Roles { get; set; }
        public string Code { get; set; }
        public string Menu { get; set; }
        public Assembly? Assembly { get; set; }
    }
}