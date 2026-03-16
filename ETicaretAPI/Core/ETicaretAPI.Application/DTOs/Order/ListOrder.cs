namespace ETicaretAPI.Application.DTOs.Order
{
    public class ListOrder
    {
        public string OrderCode { get; set; }
        public string UserName { get; set; }
        public float TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
