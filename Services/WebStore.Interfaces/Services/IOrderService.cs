using System.Collections.Generic;
using WebStore.Domain.DTO.Order;

namespace WebStore.Interfaces.Services
{
    public interface IOrderService
    {
        IEnumerable<OrderDto> GetUserOrders(string UserName);

        OrderDto GetOrderById(int id);

        OrderDto CreateOrder(CreateOrderModel model, string UserName);
    }
}
