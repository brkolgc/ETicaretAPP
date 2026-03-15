using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Order.CreateOrder
{
    public class CreateOrderCommandRequest : IRequest<CreateOrderCommandResponse>
    {
        public string? BasketId { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
    }
}