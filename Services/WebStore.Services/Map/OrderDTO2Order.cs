using System.Linq;
using WebStore.Domain.DTO.Order;
using WebStore.Domain.Entities;

namespace WebStore.Services.Map
{
    public static class OrderDTO2Order
    {
        public static OrderDTO Map(this Order order) =>
            new OrderDTO
            {
                Id = order.Id,
                Name = order.Name,
                Address = order.Address,
                Date = order.Date,
                Phone = order.Phone,
                Items = order.OrderItems?.Select(item => new OrderItemDTO
                {
                    Id = item.Id,
                    Quantity = item.Count
                })
            };
    }
}
