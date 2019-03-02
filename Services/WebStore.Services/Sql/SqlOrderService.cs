using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.DTO.Order;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Map;

namespace WebStore.Services.Sql
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStoreContext _db;
        private readonly UserManager<User> _UserManager;

        public SqlOrderService(WebStoreContext db, UserManager<User> UserManager)
        {
            _db = db;
            _UserManager = UserManager;
        }

        public IEnumerable<OrderDTO> GetUserOrders(string UserName)
        {
            return _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .Where(o => o.User.UserName == UserName)
                .ToArray()
                .Select(OrderDTO2Order.Map);
        }

        public OrderDTO GetOrderById(int id)
        {
            return _db.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == id).Map();
        }

        public OrderDTO CreateOrder(CreateOrderModel Model, string UserName)
        {
            var user = _UserManager.FindByNameAsync(UserName).Result;
            using (var transaction = _db.Database.BeginTransaction())
            {
                var OrderModel = Model.OrderViewModel;
                var order = new Order
                {
                      Address =  OrderModel.Address,
                      Name = OrderModel.Name,
                      User = user,
                      Date = DateTime.Now,
                      Phone = OrderModel.PhoneNumber
                };

                _db.Orders.Add(order);

                foreach (var item in Model.Items)
                {
                    //var product_view_model = item.Key;
                    //var product = _db.Products.FirstOrDefault(p => p.Id == product_view_model.Id);
                    var product = _db.Products.FirstOrDefault(p => p.Id == item.Id);
                    if(product is null)
                        throw new InvalidOperationException($"Товар с id={item.Id} в базе не неайден");

                    var order_item = new OrderItem
                    {
                          Order = order,
                          Price = product.Price,
                          Count = item.Quantity,
                          Product = product
                    };

                    _db.OrderItems.Add(order_item);
                }

                _db.SaveChanges();

                transaction.Commit();

                return order.Map();
            }
        }
    }
}
