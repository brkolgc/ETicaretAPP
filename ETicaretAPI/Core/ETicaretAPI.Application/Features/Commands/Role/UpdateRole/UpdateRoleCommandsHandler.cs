using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Role.UpdateRole
{
    public class UpdateRoleCommandsHandler : IRequestHandler<UpdateRoleCommandsRequest, UpdateRoleCommandsResponse>
    {
        readonly IRoleService _roleService;

        public UpdateRoleCommandsHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<UpdateRoleCommandsResponse> Handle(UpdateRoleCommandsRequest request, CancellationToken cancellationToken)
        {
            bool result = await _roleService.UpdateRoleAsync(request.Id, request.Name);
            return new() { Succeeded = result };
        }
    }
}
