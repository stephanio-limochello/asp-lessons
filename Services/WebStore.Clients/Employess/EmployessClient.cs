using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;
	
namespace WebStore.Clients.Values
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        public EmployeesClient(IConfiguration Configuration) : base(Configuration, WebAPI.Employees) { }

        public IEnumerable<Employee> Get() => Get<IEnumerable<Employee>>(_serviceAddress);

        public Employee GetById(int id) => Get<Employee>($"{_serviceAddress}/{id}");

        public int Add(Employee employee) => Post(_serviceAddress, employee).Content.ReadAsAsync<int>().Result;

        //public void Edit(int id, Employee employee) => Put($"{_ServiceAddress}/{id}", employee);
        public void Edit(Employee employee) => Put(_serviceAddress, employee);

        public bool Delete(int id) => Delete($"{_serviceAddress}/{id}").IsSuccessStatusCode;

        public void SaveChanges() { }
    }
}