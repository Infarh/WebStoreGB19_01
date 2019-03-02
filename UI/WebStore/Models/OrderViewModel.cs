using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class OrderViewModel
    {
        [Display(Name = "Название заказа"), Required]
        public string Name { get; set; }

        [Display(Name = "Телефон"), Required, DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Адрес"), Required]
        public string Address { get; set; }
    }
}
