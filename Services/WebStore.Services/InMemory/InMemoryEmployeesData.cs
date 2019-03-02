using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Services.InMemory
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private readonly List<EmployeeViewModel> _Employes = new List<EmployeeViewModel>
        {
            new EmployeeViewModel { Id = 0, FirstName = "Иван", SecondName = "Иванов", Patronymic = "Иванович", Age = 28 },
            new EmployeeViewModel { Id = 1, FirstName = "Пётр", SecondName = "Петров", Patronymic = "Петрович", Age = 35 },
            new EmployeeViewModel { Id = 2, FirstName = "Сидор", SecondName = "Сидоров", Patronymic = "Сидорович", Age = 22 },
        };

        public InMemoryEmployeesData()
        {

        }

        public IEnumerable<EmployeeViewModel> GetAll() => _Employes;

        public EmployeeViewModel GetById(int id) => _Employes.FirstOrDefault(emp => emp.Id == id);

        public EmployeeViewModel UpdateEmployee(int id, EmployeeViewModel employe)
        {
            if(employe is null) throw new ArgumentNullException(nameof(employe));

            var exists_employee = GetById(id);
            if(exists_employee is null)
                throw new InvalidOperationException($"Сотрудник с id {id} не найден");

            exists_employee.FirstName = employe.FirstName;
            exists_employee.SecondName = employe.SecondName;
            exists_employee.Patronymic = employe.Patronymic;
            exists_employee.Age = employe.Age;

            return exists_employee;
        }

        public void AddNew(EmployeeViewModel NewEmployee)
        {
            if(_Employes.Contains(NewEmployee))
                return;
            NewEmployee.Id = _Employes.Count == 0 ? 1 : _Employes.Max(e => e.Id) + 1;
            _Employes.Add(NewEmployee);
        }

        public void Delete(int id)
        {
            var employee = GetById(id);
            if(employee is null) return;
            _Employes.Remove(employee);
        }

        public void SaveChanges() { }
    }
}
