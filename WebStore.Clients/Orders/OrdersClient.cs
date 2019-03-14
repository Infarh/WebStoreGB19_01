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

        public OrdersClient(IConfiguration configuration, ILogger<OrdersClient> logger) : base(configuration)
        {
            _Logger = logger;
            ServiceAddress = "api/orders";
        }

        public IEnumerable<OrderDto> GetUserOrders(string userName)
        {
            return Get<List<OrderDto>>($"{ServiceAddress}/user/{userName}");
        }

        public OrderDto GetOrderById(int id)
        {
            return Get<OrderDto>($"{ServiceAddress}/{id}");
        }

        public OrderDto CreateOrder(CreateOrderModel model, string userName)
        {
            _Logger.LogInformation($"Создание заказа для {userName}");
            
            var response = Post($"{ServiceAddress}/{userName}", model);
            return response.Content.ReadAsAsync<OrderDto>().Result;
        }
    }
}