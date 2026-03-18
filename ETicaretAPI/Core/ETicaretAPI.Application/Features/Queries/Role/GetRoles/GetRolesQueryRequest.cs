using ETicaretAPI.Application.RequestParameters;
using MediatR;

namespace ETicaretAPI.Application.Features.Queries.Role.GetRoles
{
    public record GetRolesQueryRequest :Pagination, IRequest<GetRolesQueryResponse>
    {
    }
}