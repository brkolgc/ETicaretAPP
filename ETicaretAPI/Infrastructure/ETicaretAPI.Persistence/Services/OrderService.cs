using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.Basket;
using ETicaretAPI.Application.DTOs.Order;
using ETicaretAPI.Application.Exceptions;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.Utilities;
using ETicaretAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Persistence.Services
{
    public class OrderService : IOrderService
    {
        readonly IOrderWriteRepository _orderWriteRepository;
        readonly IOrderReadRepository _orderReadRepository;
        readonly ICompletedOrderWriteRepository _completedOrderWriteRepository;

        public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository, ICompletedOrderWriteRepository completedOrderWriteRepository)
        {
            _orderWriteRepository = orderWriteRepository;
            _orderReadRepository = orderReadRepository;
            _completedOrderWriteRepository = completedOrderWriteRepository;
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
                   TotalPrice = o.Basket.BasketItems.Sum(bi => (float?)(bi.Product.Price * bi.Quantity)) ?? 0,
                   UserName = o.Basket.User.UserName,
                   Completed = o.CompletedOrder != null
               });

            return (await query.Skip(page * size).Take(size).ToListAsync(), await query.CountAsync());
        }

        public async Task<SingleOrder> GetOrderByIdAsync(string id)
        {
            SingleOrder? singleOrder = await _orderReadRepository.Table
                  .AsNoTracking()
                  .Where(o => o.Id == Guid.Parse(id))
                  .Select(o => new SingleOrder
                  {
                      Id = o.Id.ToString(),
                      Address = o.Address,
                      OrderCode = o.OrderCode,
                      CreatedDate = o.CreatedDate,
                      Description = o.Description,
                      Completed = o.CompletedOrder != null,
                      BasketItems = o.Basket.BasketItems.Select(bi => new OrderBasketItem()
                      {
                          Name = bi.Product.Name,
                          Price = bi.Product.Price,
                          Quantity = bi.Quantity
                      }).ToList()
                  }).FirstOrDefaultAsync();

            return singleOrder;
        }

        public async Task<(bool, CompletedOrderDTO)> CompleteOrderAsync(string id)
        {
          CompletedOrderDTO? order =  await _orderReadRepository.Table
                .AsNoTracking()
                .Where(o => o.Id == Guid.Parse(id))
                .Select(o => new CompletedOrderDTO()
                {
                    OrderCode = o.OrderCode,
                    OrderDate = o.CreatedDate,
                    NameSurname = o.Basket.User.NameSurname,
                    Email = o.Basket.User.Email
                }).FirstOrDefaultAsync();

            if (order != null)
            {
                await _completedOrderWriteRepository.AddAsync(new() { OrderId = Guid.Parse(id) });
                return (await _completedOrderWriteRepository.SaveAsync() > 0, order);
            }
            else
                throw new CompleteOrderFailedException();
        }
    }
}
