using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Role.CreateRole
{
    public class CreateRoleCommandsRequest : IRequest<CreateRoleCommandsResponse>
    {
        public string Name { get; set; }
    }
}