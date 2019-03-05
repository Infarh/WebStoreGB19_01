﻿using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IProductData _ProductsData;

        public HomeController(IProductData ProductsData)
        {
            _ProductsData = ProductsData;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProductList()
        {
            return View(_ProductsData.GetProducts());
        }

        public IActionResult Create() => View();

        public IActionResult Edit(int Id) => View();

        public IActionResult Details(int Id) => View();

        public IActionResult Delete(int Id) => View();
    }
}