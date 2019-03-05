using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO.Order;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class OrdersController : ControllerBase, IOrderService
    {
        private readonly IOrderService _OrderService;

        public OrdersController(IOrderService orderService) => _OrderService = orderService;

        [HttpGet("user/{UserName}")]
        public IEnumerable<OrderDto> GetUserOrders(string userName)
        {
            return _OrderService.GetUserOrders(userName);
        }

        [HttpGet("{id}"), ActionName("Get")]
        public OrderDto GetOrderById(int id)
        {
            return _OrderService.GetOrderById(id);
        }

        [HttpPost("{UserName?}")]
        public OrderDto CreateOrder([FromBody] CreateOrderModel model, string userName)
        {
            return _OrderService.CreateOrder(model, userName);
        }
    }
}
