using System.Linq;
using WebStore.Domain.DTO.Order;
using WebStore.Domain.Entities;

namespace WebStore.Services.Map
{
    public static class OrderDto2Order
    {
        public static OrderDto Map(this Order order)
        {
            return new OrderDto()
            {
                Id = order.Id,
                Name = order.Name,
                Address = order.Address,
                Date = order.Date,
                Phone = order.Phone,
                Items = order.OrderItems?.Select(item => new OrderItemDto()
                {
                    Id = item.Id,
                    Quantity = item.Count
                })
            };
        }
    }

}