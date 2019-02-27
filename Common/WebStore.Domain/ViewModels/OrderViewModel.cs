using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.ViewModels
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
