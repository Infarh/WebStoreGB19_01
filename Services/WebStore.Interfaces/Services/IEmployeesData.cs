using System.Collections.Generic;
using WebStore.Domain.ViewModels;

namespace WebStore.Interfaces.Services
{
    public interface IEmployeesData
    {
        IEnumerable<EmployeeViewModel> GetAll();
        
        EmployeeViewModel GetById(int id);

        EmployeeViewModel UpdateEmployee(int id, EmployeeViewModel employee);
        
        void AddNew(EmployeeViewModel newEmployee);
        
        void Delete(int id);
        
        void SaveChanges();
    }
}
