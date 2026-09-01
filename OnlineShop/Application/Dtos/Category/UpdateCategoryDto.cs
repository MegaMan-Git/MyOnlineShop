using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Category
{
    public class UpdateCategoryDto
    {
        public int Id { get; set; }
        public string NewCategoryName { get; set; } = string.Empty;
    }
}
