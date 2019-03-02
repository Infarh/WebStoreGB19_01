using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

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

        public IEnumerable<Order> GetUserOrders(string UserName)
        {
            return _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .Where(o => o.User.UserName == UserName)
                .ToArray();
        }

        public Order GetOrderById(int id)
        {
            return _db.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == id);
        }

        public Order CreateOrder(OrderViewModel OrderModel, CartViewModel CartModel, string UserName)
        {
            var user = _UserManager.FindByNameAsync(UserName).Result;
            using (var transaction = _db.Database.BeginTransaction())
            {
                var order = new Order
                {
                      Address =  OrderModel.Address,
                      Name = OrderModel.Name,
                      User = user,
                      Date = DateTime.Now,
                      Phone = OrderModel.PhoneNumber
                };

                _db.Orders.Add(order);

                foreach (var item in CartModel.Items)
                {
                    var product_view_model = item.Key;
                    //var product = _db.Products.FirstOrDefault(p => p.Id == product_view_model.Id);
                    var product = _db.Products.Find(product_view_model.Id);
                    if(product is null)
                        throw new InvalidOperationException($"Товар с id={product_view_model.Id} в базе не неайден");

                    var order_item = new OrderItem
                    {
                          Order = order,
                          Price = product.Price,
                          Count = item.Value,
                          Product = product
                    };

                    _db.OrderItems.Add(order_item);
                }

                _db.SaveChanges();

                transaction.Commit();

                return order;
            }
        }
    }
}
