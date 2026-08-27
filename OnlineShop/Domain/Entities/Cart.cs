using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Cart
{
    public Cart()
    {
        CartItems = new List<CartItem>();
    }
    public int Id { get; set; }

    public string UserId { get; set; } = null!;


    public ICollection<CartItem> CartItems { get; set; }
}
