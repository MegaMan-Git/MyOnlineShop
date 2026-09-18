using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Cart.Cartitem
{
    public class UpdateCartItemDto
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
