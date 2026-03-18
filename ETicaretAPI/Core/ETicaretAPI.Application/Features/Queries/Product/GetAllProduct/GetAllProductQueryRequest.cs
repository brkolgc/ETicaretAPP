using ETicaretAPI.Application.RequestParameters;
using MediatR;

namespace ETicaretAPI.Application.Features.Queries.Product.GetAllProduct
{
    public record GetAllProductQueryRequest :Pagination, IRequest<GetAllProductQueryResponse>
    {
    }
}
