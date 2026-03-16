using ETicaretAPI.Application.DTOs.Order;

namespace ETicaretAPI.Application.Features.Queries.Order
{
    public class GetAllOrdersQueryResponse
    {
        public int TotalOrderCount { get; set; }
        public List<ListOrder> Orders { get; set; }
    }
}