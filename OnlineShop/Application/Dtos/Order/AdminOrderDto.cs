using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Order
{
    public class AdminOrderDto
    {
       public int Id { get; set; }
        public string? UserName {  get; set; } = string.Empty;
    }
}
