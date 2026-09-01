using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Cart.Cartitem
{
    public class AdminCartItemDto
    {
        public int Id { get; set; }

        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? UserName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
