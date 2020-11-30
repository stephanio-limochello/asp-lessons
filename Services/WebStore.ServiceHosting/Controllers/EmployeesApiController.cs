using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.ServiceHosting.Controllers
{
	[Route(WebAPI.Employees)]
    [ApiController]
    public class EmployeesApiController : ControllerBase, IEmployeesData
    {
        private readonly IEmployeesData _employeesData;
		private readonly ILogger<EmployeesApiController> logger;

		public EmployeesApiController(IEmployeesData employeesData, ILogger<EmployeesApiController> logger)
		{
            _employeesData = employeesData;
			this.logger = logger;
		}

        /// <summary>Getting a list of all employees</summary>
        /// <returns>List of all employees</returns>
        [HttpGet]
        public IEnumerable<Employee> Get() => _employeesData.Get();

        /// <summary>Getting an employee by his ID</summary>
        /// <param name="id">ID of the employee of interest</param>
        /// <returns>Employee with the specified ID</returns>
        [HttpGet("{id}")]
        public Employee GetById(int id) => _employeesData.GetById(id);

        /// <summary>Add new employee</summary>
        /// <param name="employee">New employee</param>
        /// <returns>The ID assigned to the employee</returns>
        [HttpPost]
        public int Add([FromBody] Employee employee)
        {
            var id = _employeesData.Add(employee);
            SaveChanges();
            return id;
        }


        /// <summary>Editing an employee</summary>
        /// <param name="employee">Editable employee</param>
        [HttpPut]
        public void Edit(Employee employee)
        {
            _employeesData.Edit(employee);
            SaveChanges();
        }

        /// <summary>Removing an employee by identifier</summary>
        /// <param name="id">Identifier for the deletion of the employee</param>
        /// <returns>True if the employee was deleted</returns>
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            var result = _employeesData.Delete(id);
            SaveChanges();
            return result;
        }

        /// <summary>Save Changes</summary>
        [NonAction]
        public void SaveChanges() => _employeesData.SaveChanges();
    }
}