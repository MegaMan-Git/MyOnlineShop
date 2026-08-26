using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int UnitPrice { get; set; }

    public int Quantity { get; set; }
}
