using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Order
{
    public Order()
    {
        OrderItems = [];
    }
    public int Id { get; set; }
    public string UserId { get; set; } = null!;

    public Payment? Payment { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}
