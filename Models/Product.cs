using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VUTApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int Weight { get; set; }
        public int Amount { get; set; }        
        public int Rating { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int ProducerId { get; set; }
        public Producer Producer { get; set; }
    }
}
