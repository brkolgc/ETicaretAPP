using ETicaretAPI.Application.DTOs.Order;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrder createOrder);
        Task<SingleOrder> GetOrderByIdAsync(string id);
        Task<(List<ListOrder> listOrders, int orderCount)> GetAllOrdersAsync(int page, int size);
    }
}
