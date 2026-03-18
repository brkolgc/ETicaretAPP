using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Role.DeleteRole
{
    public class DeleteRoleCommandsHandler : IRequestHandler<DeleteRoleCommandsRequest, DeleteRoleCommandsResponse>
    {
        readonly IRoleService _roleService;

        public DeleteRoleCommandsHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<DeleteRoleCommandsResponse> Handle(DeleteRoleCommandsRequest request, CancellationToken cancellationToken)
        {
            bool result = await _roleService.DeleteRoleAsync(request.Id);
            return new() { Succeeded = result };
        }
    }
}
