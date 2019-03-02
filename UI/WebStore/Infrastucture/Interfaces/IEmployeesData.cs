using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Infrastucture.Interfaces
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
