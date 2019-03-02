using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{

    //[ApiController, Route("api/Employees")]
    [ApiController, Route("api/[controller]"), Produces("application/json")]
    public class EmployeesController : ControllerBase, IEmployeesData
    {
        private readonly IEmployeesData _EmployeesData;

        public EmployeesController(IEmployeesData EmployeesData) => _EmployeesData = EmployeesData;

        [HttpGet, ActionName("Get")]
        public IEnumerable<EmployeeViewModel> GetAll()
        {
            return _EmployeesData.GetAll();
        }

        [HttpGet("{id}"), ActionName("Get")]
        public EmployeeViewModel GetById(int id)
        {
            return _EmployeesData.GetById(id);
        }

        [HttpPut("{id}"), ActionName("Put")]
        public EmployeeViewModel UpdateEmployee(int id, [FromBody] EmployeeViewModel employee)
        {
            return _EmployeesData.UpdateEmployee(id, employee);
        }

        [HttpPost, ActionName("Post")]
        public void AddNew([FromBody] EmployeeViewModel NewEmployee)
        {
            _EmployeesData.AddNew(NewEmployee);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _EmployeesData.Delete(id);
        }

        [NonAction]
        public void SaveChanges()
        {
            _EmployeesData.SaveChanges();
        }
    }
}