using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Payment
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int? Amount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
