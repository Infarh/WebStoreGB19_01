using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Domain.DTO.Order;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Orders
{
    public class OrdersClient : BaseClient, IOrderService
    {
        public OrdersClient(IConfiguration configuration) : base(configuration) => ServiceAddress = "api/orders";

        #region Implementation of IOrderService

        public IEnumerable<OrderDTO> GetUserOrders(string UserName)
        {
            return Get<List<OrderDTO>>($"{ServiceAddress}/user/{UserName}");
        }

        public OrderDTO GetOrderById(int id)
        {
            return Get<OrderDTO>($"{ServiceAddress}/{id}");
        }

        public OrderDTO CreateOrder(CreateOrderModel Model, string UserName)
        {
            var response = Post($"{ServiceAddress}/{UserName}", Model);
            return response.Content.ReadAsAsync<OrderDTO>().Result;
        }

        #endregion
    }
}
