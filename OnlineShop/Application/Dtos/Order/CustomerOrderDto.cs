using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Order
{
    public class CustomerOrderDto
    {
        public int OrderId { get; set; }
        public string OrderName { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Authority { get; set; }
        public string? ReferenceId { get; set; }
    }
}
