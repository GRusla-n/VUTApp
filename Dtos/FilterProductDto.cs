using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VUTApp.Dtos
{
    public class FilterProductDto : PaginationDto
    {                
        public string Name { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }        
        public string CategoryName { get; set; }
        public string OrderingField { get; set; }
        public bool AscendingOrder { get; set; } = true;
    }
}
