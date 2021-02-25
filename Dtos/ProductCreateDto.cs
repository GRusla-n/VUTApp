using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VUTApp.Models;
using VUTApp.Validation;

namespace VUTApp.Dtos
{
    public class ProductCreateDto
    {        
        public string Name { get; set; }

        [FileSizeValidator(5)]
        [ContentTypeValidator(ContentTypeGroup.Image)]
        public IFormFile Image { get; set; }
        
        public double Price { get; set; }

        public int Weight { get; set; }
        public int Amount { get; set; }
        public int CategoryId { get; set; }
        public int ProducerId { get; set; }
        public int Rating { get; set; }
    }
}
