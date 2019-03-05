using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class EmployeesController : ControllerBase, IEmployeesData
    {
        private readonly IEmployeesData _EmployeesData;

        public EmployeesController(IEmployeesData employeesData) => _EmployeesData = employeesData;
        
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
        public void AddNew([FromBody] EmployeeViewModel newEmployee)
        {
            _EmployeesData.AddNew(newEmployee);
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