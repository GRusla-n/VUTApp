using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VUTApp.Models
{
    public class Producer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }           
    }
}
