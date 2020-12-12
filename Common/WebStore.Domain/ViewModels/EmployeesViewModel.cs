using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Domain.ViewModels
{
    public class EmployeesViewModel //: IValidatableObject
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Длина строи имени должна быть от 3 до 200 символов")]
        //[RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Ошибка формата имени. Либо русские буквы, либо латиница.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Длина строи фамилии должна быть от 3 до 200 символов")]
        public string LastName { get; set; }

        [Display(Name = "Patronymic")]
        public string Patronymic { get; set; }

        [Display(Name = "Age")]
        [Required(ErrorMessage = "Age is required")]
        [Range(20, 80, ErrorMessage = "Age должен быть в пределах от 20 до 80 лет")]
        public int Age { get; set; }


        /// <summary>
        /// Employment Date
        /// </summary>
        [Display(Name = "Employment Date")]
        [DataType(DataType.DateTime)]
        public DateTime EmployementDate { get; set; }
    }
}
