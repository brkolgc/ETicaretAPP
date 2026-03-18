using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Queries.Role.GetRoles
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQueryRequest, GetRolesQueryResponse>
    {
        readonly IRoleService _roleService;

        public GetRolesQueryHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task<GetRolesQueryResponse> Handle(GetRolesQueryRequest request, CancellationToken cancellationToken)
        {
            (IDictionary<string,string> roles, int totalRoleCount) = await _roleService.GetAllRolesAsync(request.Page,request.Size);
            return new()
            {
                Roles = roles,
                TotalRoleCount = totalRoleCount
            };
        }
    }
}
