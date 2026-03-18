using ETicaretAPI.Application.RequestParameters;
using MediatR;

namespace ETicaretAPI.Application.Features.Queries.Order.GetAllOrders
{
    public record GetAllOrdersQueryRequest :Pagination, IRequest<GetAllOrdersQueryResponse>
    {
    }
}