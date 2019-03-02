using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO.Order;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [ApiController, Route("api/[controller]"), Produces("application/json")]
    public class OrdersController : ControllerBase, IOrderService
    {
        private readonly IOrderService _OrderService;

        public OrdersController(IOrderService OrderService) => _OrderService = OrderService;

        #region Implementation of IOrderService

        [HttpGet("user/{UserName}")]
        public IEnumerable<OrderDTO> GetUserOrders(string UserName)
        {
            return _OrderService.GetUserOrders(UserName);
        }

        [HttpGet("{id}"), ActionName("Get")]
        public OrderDTO GetOrderById(int id)
        {
            return _OrderService.GetOrderById(id);
        }

        [HttpPost("{UserName?}")]
        public OrderDTO CreateOrder([FromBody] CreateOrderModel Model, string UserName)
        {
            return _OrderService.CreateOrder(Model, UserName);
        }

        #endregion
    }
}