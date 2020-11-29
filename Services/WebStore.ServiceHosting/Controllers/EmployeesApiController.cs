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

        [HttpGet]
        public IEnumerable<Employee> Get() => _employeesData.Get();

        [HttpGet("{id}")]
        public Employee GetById(int id) => _employeesData.GetById(id);

        [HttpPost]
        public int Add([FromBody] Employee employee)
        {
            var id = _employeesData.Add(employee);
            SaveChanges();
            return id;
        }

        [HttpPut]
        public void Edit(Employee employee)
        {
            _employeesData.Edit(employee);
            SaveChanges();
        }

        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            var result = _employeesData.Delete(id);
            SaveChanges();
            return result;
        }

        [NonAction]
        public void SaveChanges() => _employeesData.SaveChanges();
    }
}