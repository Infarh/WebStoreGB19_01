using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        public EmployeesClient(IConfiguration configuration) : base(configuration)
        {
            ServiceAddress = "api/Employees"; 
        }

        public IEnumerable<EmployeeViewModel> GetAll()
        {
            return Get<List<EmployeeViewModel>>(ServiceAddress);
        }

        public EmployeeViewModel GetById(int id)
        {
            return Get<EmployeeViewModel>($"{ServiceAddress}/{id}");
        }

        public EmployeeViewModel UpdateEmployee(int id, EmployeeViewModel employee)
        {
            var response = Put($"{ServiceAddress}/{id}", employee);

            return response.Content.ReadAsAsync<EmployeeViewModel>().Result;
        }

        public void AddNew(EmployeeViewModel newEmployee)
        {
            Post(ServiceAddress, newEmployee);
        }

        public void Delete(int id)
        {
            Delete($"{ServiceAddress}/{id}");
        }

        public void SaveChanges()
        {
        }
    }
}