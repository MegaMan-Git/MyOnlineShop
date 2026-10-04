using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Order
{
    public class AdminOrderDto
    {
       public int OrderId { get; set; }
        public string? UserName {  get; set; } = string.Empty;
    }
}
