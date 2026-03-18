using ETicaretAPI.Application.RequestParameters;
using MediatR;

namespace ETicaretAPI.Application.Features.Queries.AppUser.GetAllUsers
{
    public record GetAllUsersQueryRequest : Pagination, IRequest<GetAllUsersQueryResponse>
    {
    }
}