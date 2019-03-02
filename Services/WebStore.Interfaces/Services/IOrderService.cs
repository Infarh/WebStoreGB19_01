using System.Collections.Generic;
using WebStore.Domain.DTO.Order;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;

namespace WebStore.Interfaces.Services
{
    public interface IOrderService
    {
        IEnumerable<OrderDTO> GetUserOrders(string UserName);

        OrderDTO GetOrderById(int id);

        OrderDTO CreateOrder(CreateOrderModel Model, string UserName);
    }
}
