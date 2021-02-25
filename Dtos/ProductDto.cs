using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VUTApp.Models;

namespace VUTApp.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int Weight { get; set; }
        public int Amount { get; set; }
        public CategoryDto Category { get; set; }
        public Producer Producer { get; set; }
        public int Rating { get; set; }
    }
}
