using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Role.UpdateRole
{
    public class UpdateRoleCommandsRequest : IRequest<UpdateRoleCommandsResponse>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}