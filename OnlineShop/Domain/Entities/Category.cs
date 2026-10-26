using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Entities;

public class Category
{
    public Category()
    {
        Products = [];
    }
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public bool IsDeleted { get; set; }

    public ICollection<Product> Products { get; set; }
}
