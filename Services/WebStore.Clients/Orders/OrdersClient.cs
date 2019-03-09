using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebStore.Clients.Base;
using WebStore.Domain.DTO.Order;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Orders
{
    public class OrdersClient : BaseClient, IOrderService
    {
        private readonly ILogger<OrdersClient> _Logger;

        public OrdersClient(IConfiguration configuration, ILogger<OrdersClient> Logger) 
            : base(configuration)
        {
            _Logger = Logger;
            ServiceAddress = "api/orders";
        }

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
            _Logger.LogInformation($"Создание заказа для {UserName}");

            var response = Post($"{ServiceAddress}/{UserName}", Model);
            return response.Content.ReadAsAsync<OrderDTO>().Result;
        }

        #endregion
    }
}
