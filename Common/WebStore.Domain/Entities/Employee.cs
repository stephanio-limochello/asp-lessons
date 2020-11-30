using System;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities
{
    /// <summary>Employee</summary>
    public class Employee : NamedEntity
    {
        /// <summary>Surname</summary>
        public string Surname { get; set; }

        /// <summary>Patronymic</summary>
        public string Patronymic { get; set; }

        /// <summary>Age</summary>
        public int Age { get; set; }

        /// <summary>Date of employment</summary>
        public DateTime EmployementDate { get; set; }
    }
}
