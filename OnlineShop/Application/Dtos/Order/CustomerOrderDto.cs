using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Order
{
    public class CustomerOrderDto
    {
        public int OrderId { get; set; }
        public string OrderName { get; set; } = string.Empty;
    }
}
