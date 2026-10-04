using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Cart.Cartitem
{
    public class AddCartItemDto
    {
       public  int ProductId { get; set; }
       public int Quantity { get; set; }
    }
}
