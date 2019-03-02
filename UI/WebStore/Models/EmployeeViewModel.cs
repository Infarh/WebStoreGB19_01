using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Models
{
    public class EmployeeViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Имя обязательно")]
        [Display(Name = "Имя")]
        [RegularExpression("^[A-ZА-Я][a-zа-я]*", ErrorMessage = "Имя должно начинаться с заглавной буквы и содержать только буквы")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        //[UIHint("SecondNameValueView")] // Пользовательское представление данных свойства в разметке
        public string SecondName { get; set; }

        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Не указан возраст")]
        [Display(Name = "Возраст")]
        [Range(minimum: 18, maximum:120, ErrorMessage = "Возраст должен быть в диапазоне от 18 до 150 лет")]
        public int Age { get; set; }

        //[Display(Name = "Многострочные данные")]
        //[DataType(DataType.MultilineText)]
        //public string MultilineData { get; set; }
    }
}
