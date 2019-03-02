using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebStore.Domain.Entities;
using WebStore.Infrastucture.Interfaces;
using WebStore.Models;

namespace WebStore.Infrastucture.Implementations
{
    public class CookieCartService : ICartService
    {
        private readonly IProductData _ProductData;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly string _CartName;

        private Cart Cart
        {
            get
            {
                var context = _HttpContextAccessor.HttpContext;
                var cookie = context.Request.Cookies[_CartName];
                Cart cart = null;
                if (cookie is null)
                {
                    cart = new Cart();
                    context.Response.Cookies.Append(_CartName, JsonConvert
                            .SerializeObject(cart),
                        new CookieOptions
                        {
                            Expires = DateTime.Now.AddDays(2)
                        });
                }
                else
                {
                    cart = JsonConvert.DeserializeObject<Cart>(cookie);
                    context.Response.Cookies.Delete(_CartName);
                    context.Response.Cookies.Append(_CartName, cookie,
                        new CookieOptions
                        {
                            Expires = DateTime.Now.AddDays(2)
                        });
                }
                return cart;
            }
            set
            {
                var context = _HttpContextAccessor.HttpContext;

                var cart_json = JsonConvert.SerializeObject(value);
                context.Response.Cookies.Delete(_CartName);
                context.Response.Cookies.Append(_CartName, cart_json,
                    new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(2)
                    });
            }
        }

        public CookieCartService(IProductData ProductData, IHttpContextAccessor HttpContextAccessor)
        {
            _ProductData = ProductData;
            _HttpContextAccessor = HttpContextAccessor;
            var user_identity = HttpContextAccessor.HttpContext.User.Identity;
            _CartName = $"{(user_identity.IsAuthenticated ? user_identity.Name : null)}";
        }

        public void DecrementFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is null) return;
            if (item.Quantity > 0)
                item.Quantity--;
            if (item.Quantity == 0)
                cart.Items.Remove(item);
            Cart = cart;
        }

        public void RemoveFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is null) return;
            cart.Items.Remove(item);
            Cart = cart;
        }

        public void RemoveAll() => Cart = new Cart();

        public void AddToCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is null)
                cart.Items.Add(new CartItem {ProductId = id, Quantity = 1});
            else
                item.Quantity++;
            Cart = cart;
        }

        public CartViewModel TransformCart()
        {
            var products = _ProductData.GetProducts(new ProductFilter
            {
                Ids = Cart.Items.Select(i => i.ProductId).ToArray()    
            }).Select(p => new ProductViewModel
            {
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                Name = p.Name,
                Brand = p.Brand?.Name ?? string.Empty,
                Order = p.Order,
                Price = p.Price
            }).ToArray();

            return new CartViewModel
            {
                Items = Cart.Items.ToDictionary(
                    i => products.First(p => p.Id == i.ProductId),
                    i => i.Quantity)
            };
        }
    }
}
