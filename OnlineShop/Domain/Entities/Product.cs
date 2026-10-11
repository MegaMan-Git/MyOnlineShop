using Application.Entities;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Product
{
    public Product()
    {
        CartItems = [];
        OrderItems = [];
    }

    public int Id { get; set; }
    public string ProductName { get; set; } = null!;
    public int Price { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public int? CategoryId { get; set; }
    public int Stock { get; set; }
    public bool IsDeleted { get; set; }

    public Category? Category { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
    public ICollection<CartItem> CartItems { get; set; }
}
