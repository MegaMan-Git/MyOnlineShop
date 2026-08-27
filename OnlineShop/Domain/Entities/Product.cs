using Application.Entities;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Product
{
    public Product()
    {
        cartItems = new List<CartItem>();
        orderItems = new List<OrderItem>();
    }

    public int Id { get; set; }

    public string ProductName { get; set; } = null!;

    public int Price { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public int? CategoryId { get; set; }

    public int Stock { get; set; }


    public Category? Category { get; set; }
    public ICollection<OrderItem> orderItems { get; set; }
    public ICollection<CartItem> cartItems { get; set; }
}
