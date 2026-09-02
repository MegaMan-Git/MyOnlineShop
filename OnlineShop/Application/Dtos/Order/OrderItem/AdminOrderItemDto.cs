using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Order.OrderItem
{
    public class AdminOrderItemDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string? UserName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Price { get; set; }
        public int TotalPrice { get; set; }
        public int Quantity { get; set; }
    }
}
