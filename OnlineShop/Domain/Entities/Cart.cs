using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Cart
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;
}
