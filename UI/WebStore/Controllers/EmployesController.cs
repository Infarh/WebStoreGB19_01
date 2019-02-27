using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces;

namespace WebStore.Controllers
{
    //[Route("Users")]
    //[TestActionFilter]
    //[ServiceFilter(typeof(TestResultFilter))]
    [Authorize]
    public class EmployesController : Controller
    {
        private readonly IEmployeesData _EmployeesData;

        public EmployesController(IEmployeesData EmployeesData)
        {
            _EmployeesData = EmployeesData;
        }

        //[Route("Get")]
        //[TestActionFilter]
        public IActionResult Index()
        {
            return View(_EmployeesData.Get());
        }

        public IActionResult Details(int? id)
        {
            if (id is null)
                return BadRequest();

            var employee = _EmployeesData.GetById((int)id);
            if (employee is null)
                return NotFound();

            return View(employee);
        }

        [HttpGet]
        [Authorize(Roles = Domain.Entities.User.AdminRole)]
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new EmployeeViewModel
                {
                    FirstName = "Employee_name",
                    SecondName = "Employee_second_name",
                    Age = 18
                });

            var employee = _EmployeesData.GetById((int)id);
            if (employee is null)
                return NotFound();

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Domain.Entities.User.AdminRole)]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (model.Age % 2 == 0)
                    ModelState.AddModelError("Ошибка возраста", "Возраст должен быть нечётным");
                return View(model);
            }
            if (model.Id == 0)
            {
                _EmployeesData.AddNew(model);
            }
            else
            {
                var employee = _EmployeesData.GetById(model.Id);
                if (employee is null)
                    return NotFound();

                employee.FirstName = model.FirstName;
                employee.SecondName = model.SecondName;
                employee.Patronymic = model.Patronymic;
                employee.Age = model.Age;
            }

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [Authorize(Roles = Domain.Entities.User.AdminRole)]
        public IActionResult Delete(int? id)
        {
            if (id is null)
                return BadRequest();

            if (_EmployeesData.GetById((int)id) is null)
                return NotFound();

            _EmployeesData.Delete((int)id);

            return RedirectToAction("Index");
        }
    }
}