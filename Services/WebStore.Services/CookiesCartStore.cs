using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Services
{
    public class CookiesCartStore : ICartStore
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly string _CartName;

        public Cart Cart
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

        public CookiesCartStore(IHttpContextAccessor HttpContextAccessor)
        {
            _HttpContextAccessor = HttpContextAccessor;
            var user_identity = HttpContextAccessor.HttpContext.User.Identity;
            _CartName = $"{(user_identity.IsAuthenticated ? user_identity.Name : null)}";
        }
    }
}
