using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Order
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;
}
