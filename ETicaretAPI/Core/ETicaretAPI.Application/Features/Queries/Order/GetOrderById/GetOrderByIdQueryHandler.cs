using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.Order;
using MediatR;

namespace ETicaretAPI.Application.Features.Queries.Order.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQueryRequest, GetOrderByIdQueryResponse>
    {
        readonly IOrderService _orderService;

        public GetOrderByIdQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQueryRequest request, CancellationToken cancellationToken)
        {
           SingleOrder singleOrder = await _orderService.GetOrderByIdAsync(request.Id);

            return new()
            {
                Id = singleOrder.Id,
                Address = singleOrder.Address,
                BasketItems = singleOrder.BasketItems, 
                CreatedDate = singleOrder.CreatedDate,
                Description = singleOrder.Description,
                OrderCode = singleOrder.OrderCode,
                Completed=singleOrder.Completed,
            };
        }
    }
}
