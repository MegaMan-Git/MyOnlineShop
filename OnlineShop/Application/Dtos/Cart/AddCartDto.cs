using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Cart
{
    public class AddCartDto
    {
       public  int ProductId { get; set; }
       public int Quantity { get; set; }
    }
}
