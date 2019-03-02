using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        public EmployeesClient(IConfiguration configuration) : base(configuration) => 
            ServiceAddress = "api/employees";

        #region Implementation of IEmployeesData

        public IEnumerable<EmployeeViewModel> GetAll()
        {
            return Get<List<EmployeeViewModel>>(ServiceAddress);
        }

        public EmployeeViewModel GetById(int id)
        {                                    //api/employees/{id}
            return Get<EmployeeViewModel>($"{ServiceAddress}/{id}");
        }

        public EmployeeViewModel UpdateEmployee(int id, EmployeeViewModel employee)
        {                         //api/employees/{id}
            var response = Put($"{ServiceAddress}/{id}", employee);
            return response.Content.ReadAsAsync<EmployeeViewModel>().Result;
        }

        public void AddNew(EmployeeViewModel NewEmployee)
        {
            Post(ServiceAddress, NewEmployee);
        }

        public void Delete(int id)
        {
            Delete($"{ServiceAddress}/{id}");
        }

        public void SaveChanges()
        {

        }

        #endregion
    }
}
