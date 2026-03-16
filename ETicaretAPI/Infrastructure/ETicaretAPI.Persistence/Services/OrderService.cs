using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.Basket;
using ETicaretAPI.Application.DTOs.Order;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.Utilities;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Persistence.Services
{
    public class OrderService : IOrderService
    {
        readonly IOrderWriteRepository _orderWriteRepository;
        readonly IOrderReadRepository _orderReadRepository;

        public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository)
        {
            _orderWriteRepository = orderWriteRepository;
            _orderReadRepository = orderReadRepository;
        }

        public async Task CreateOrderAsync(CreateOrder createOrder)
        {
            await _orderWriteRepository.AddAsync(new()
            {
                Address = createOrder.Address,
                Id = Guid.Parse(createOrder.BasketId),
                Description = createOrder.Description,
                OrderCode = OrderCodeGenerator.GenerateOrderCode()
            });

            await _orderWriteRepository.SaveAsync();
        }

        public async Task<(List<ListOrder> listOrders, int orderCount)> GetAllOrdersAsync(int page, int size)
        {
            var query = _orderReadRepository.Table
                .AsNoTracking()
               .Select(o => new ListOrder
               {
                   Id = o.Id.ToString(),
                   CreatedDate = o.CreatedDate,
                   OrderCode = o.OrderCode,
                   TotalPrice = o.Basket.BasketItems.Sum(bi => bi.Product.Price * bi.Quantity),
                   UserName = o.Basket.User.UserName
               });

            return (await query.Skip(page * size).Take(size).ToListAsync(), await query.CountAsync());
        }

        public async Task<SingleOrder> GetOrderByIdAsync(string id)
        {
            SingleOrder? singleOrder = await _orderReadRepository.Table
                  .AsNoTracking()
                  .Where(o=>o.Id == Guid.Parse(id))
                  .Select(o => new SingleOrder
                  {
                      Id = o.Id.ToString(),
                      Address = o.Address,
                      OrderCode = o.OrderCode,
                      CreatedDate = o.CreatedDate,
                      Description = o.Description,
                      BasketItems = o.Basket.BasketItems.Select(bi => new OrderBasketItem()
                      {
                          Name = bi.Product.Name,
                          Price = bi.Product.Price,
                          Quantity = bi.Quantity
                      }).ToList()
                  }).FirstOrDefaultAsync();

            return singleOrder;
        }
    }
}
