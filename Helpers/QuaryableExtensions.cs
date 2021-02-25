using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VUTApp.Dtos;

namespace VUTApp.Helpers
{
    public static class QuaryableExtensions
    {
        public static IQueryable<T> Paginate<T> (this IQueryable<T> queryable, PaginationDto pagination)
        {            
            return queryable
                .Skip((pagination.Page - 1) * pagination.RecordsPerPage)
                .Take(pagination.RecordsPerPage);
        }
    }
}
