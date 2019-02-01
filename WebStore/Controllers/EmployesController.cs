using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class EmployesController : Controller
    {
        private static readonly List<EmployeeViewModel> __Employes = new List<EmployeeViewModel>
        {
            new EmployeeViewModel { Id = 0, FirstName = "Иван", SecondName = "Иванов", Patronymic = "Иванович", Age = 28 },
            new EmployeeViewModel { Id = 0, FirstName = "Пётр", SecondName = "Петров", Patronymic = "Петрович", Age = 35 },
            new EmployeeViewModel { Id = 0, FirstName = "Сидор", SecondName = "Сидоров", Patronymic = "Сидорович", Age = 22 },
        };

        public IActionResult Index()
        {
            //ViewBag.Title = "";
            //ViewData["Title"] = "123";
            return View(__Employes);
        }
    }
}