using System;
using System.Collections.Generic;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.ViewModels;

namespace WebStore.Domain.DTO.Order
{
    public class OrderDTO : NamedEntity
    {
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<OrderItemDTO> Items { get; set; }
    }

    public class OrderItemDTO : BaseEntity
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateOrderModel
    {
        public OrderViewModel OrderViewModel { get; set; }
        public List<OrderItemDTO> Items { get; set; }
    }
}
