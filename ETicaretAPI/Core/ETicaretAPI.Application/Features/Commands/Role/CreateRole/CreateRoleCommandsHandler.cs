using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Role.CreateRole
{
    public class CreateRoleCommandsHandler : IRequestHandler<CreateRoleCommandsRequest, CreateRoleCommandsResponse>
    {
        readonly IRoleService _roleService;

        public CreateRoleCommandsHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<CreateRoleCommandsResponse> Handle(CreateRoleCommandsRequest request, CancellationToken cancellationToken)
        {
            bool result = await _roleService.CreateRoleAsync(request.Name);

            return new() { Succeeded = result };
        }
    }
}
