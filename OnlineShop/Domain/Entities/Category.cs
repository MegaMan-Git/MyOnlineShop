using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Entities;

public class Category
{
    public Category()
    {
        products = new List<Product>();
    }
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public ICollection<Product> products { get; set; }
}
