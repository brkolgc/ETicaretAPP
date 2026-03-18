using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Role.DeleteRole
{
    public class DeleteRoleCommandsRequest : IRequest<DeleteRoleCommandsResponse>
    {
        public string Id { get; set; }
    }
}