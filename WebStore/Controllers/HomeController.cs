using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private static List<EmployeeViewModel> _Employes = new List<EmployeeViewModel>
        {
            new EmployeeViewModel { Id = 0, FirstName = "Иван", SecondName = "Иванов", Patronymic = "Иванович", Age = 28 },
            new EmployeeViewModel { Id = 1, FirstName = "Пётр", SecondName = "Петров", Patronymic = "Петрович", Age = 35 },
            new EmployeeViewModel { Id = 2, FirstName = "Сидор", SecondName = "Сидоров", Patronymic = "Сидорович", Age = 22 },
        };

        public HomeController()
        {

        }

        public IActionResult Index()
        {
            return View(_Employes);
        }  
    }
}