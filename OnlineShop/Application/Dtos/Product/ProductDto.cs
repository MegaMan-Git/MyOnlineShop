using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int? CategoryId { get; set; }
        public int Stock { get; set; }
    }
}
