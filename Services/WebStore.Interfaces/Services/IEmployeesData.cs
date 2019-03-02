using System.Collections.Generic;
using WebStore.Domain.ViewModels;

namespace WebStore.Interfaces.Services
{
    public interface IEmployeesData
    {
        IEnumerable<EmployeeViewModel> Get();
        EmployeeViewModel GetById(int id);
        void AddNew(EmployeeViewModel NewEmployee);
        void Delete(int id);
        void SaveChanges();
    }
}
