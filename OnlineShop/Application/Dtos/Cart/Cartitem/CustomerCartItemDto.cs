using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Cart.Cartitem
{
    public class CustomerCartItemDto
    {
        public int Id { get; set; }
        public int CartId {  get; set; }
        public int ProductId {  get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int TotalPrice { get; set; }
    }
}
