using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.DTO.Order;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _CartService;
        private readonly IOrderService _OrderService;

        public CartController(ICartService CartService, IOrderService OrderService)
        {
            _CartService = CartService;
            _OrderService = OrderService;
        }

        public IActionResult GetCartView() => ViewComponent("Cart");

        public IActionResult Details()
        {
            return View(new DetailsViewModel
            {
                CartViewModel = _CartService.TransformCart(),
                OrderViewModel = new OrderViewModel()
            });
        }

        public IActionResult DecrementFromCart(int Id)
        {
            _CartService.DecrementFromCart(Id);
            return Json(new { Id, message = "Количество товара уменьшено на 1" });
        }

        public IActionResult RemoveFromCart(int Id)
        {
            _CartService.RemoveFromCart(Id);
            return Json(new { Id, message = "Товар удалён из корзины" });
        }

        public IActionResult RemoveAll()
        {
            _CartService.RemoveAll();
            return RedirectToAction("Details");
        }

        public IActionResult AddToCart(int Id)
        {
            _CartService.AddToCart(Id);
            return Json(new { Id, message = "Товар добавлен в корзину" });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CheckOut(OrderViewModel model, [FromServices] ILogger<CartController> logger)
        {
            if (!ModelState.IsValid)
                return View("Details", new DetailsViewModel
                {
                    CartViewModel = _CartService.TransformCart(),
                    OrderViewModel = model
                });

            logger.LogInformation("Оформление заказа...");

            var create_order_model = new CreateOrderModel
            {
                OrderViewModel = model,
                Items = _CartService.TransformCart().Items.Select(item => new OrderItemDTO
                {
                    Id = item.Key.Id,
                    Quantity = item.Value
                }).ToList()
            };
            var order = _OrderService.CreateOrder(create_order_model, User.Identity.Name);
            _CartService.RemoveAll();
            return RedirectToAction("OrderConfirmed", new { id = order.Id });
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}