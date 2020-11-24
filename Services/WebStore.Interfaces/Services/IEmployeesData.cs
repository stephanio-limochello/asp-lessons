using System.Collections.Generic;
using WebStore.Domain.Entities;

namespace WebStore.Services.Interfaces
{
    public interface IEmployeesData
    {
        IEnumerable<Employee> Get();

        Employee GetById(int id);

        int Add(Employee employee);

        void Edit(Employee employee);

        bool Delete(int id);

        void SaveChanges();
    }
}
