using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.DTO.Order;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Map;

namespace WebStore.Services.Sql
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStoreContext _Db;
        private readonly UserManager<User> _UserManager;

        public SqlOrderService(WebStoreContext db, UserManager<User> userManager)
        {
            _Db = db;
            _UserManager = userManager;
        }

        public IEnumerable<OrderDto> GetUserOrders(string userName)
        {
            return _Db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .Where(o => o.User.UserName == userName)
                .ToArray()
                .Select(OrderDto2Order.Map);
        }

        public OrderDto GetOrderById(int id)
        {
            return _Db.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == id).Map();
        }

        public OrderDto CreateOrder(CreateOrderModel model, string userName)
        {
            var user = _UserManager.FindByNameAsync(userName).Result;
            using (var transaction = _Db.Database.BeginTransaction())
            {
                var orderModel = model.OrderViewModel;
                var order = new Order
                {
                      Address =  orderModel.Address,
                      Name = orderModel.Name,
                      User = user,
                      Date = DateTime.Now,
                      Phone = orderModel.PhoneNumber
                };

                _Db.Orders.Add(order);

                foreach (var item in model.Items)
                {
//                    var product_view_model = item.Key;
                    //var product = _db.Products.FirstOrDefault(p => p.Id == product_view_model.Id);
                    var product = _Db.Products.FirstOrDefault(p => p.Id == item.Id);
                    if(product is null)
                        throw new InvalidOperationException($"Товар с id={item.Id} в базе не неайден");

                    var order_item = new OrderItem
                    {
                          Order = order,
                          Price = product.Price,
                          Count = item.Quantity,
                          Product = product
                    };

                    _Db.OrderItems.Add(order_item);
                }

                _Db.SaveChanges();

                transaction.Commit();

                return order.Map();
            }
        }
    }
}
