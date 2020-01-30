using System;
using System.Collections.Generic;
using System.Text;

namespace LTTestProject.Models.DTOs
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public CategoryDTO Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUri { get; set; }
    }
}
