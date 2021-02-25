using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace VUTApp.Helpers
{
    public static class HttpContextExtension
    {
        public async static Task InsertPaginationParametersInResponse<T> (this HttpContext httpContext, IQueryable<T> queryable, int recordsPerPage)
        {
            if (httpContext == null) { throw new ArgumentNullException(nameof(httpContext));  }

            double count = await queryable.CountAsync();
            double totalAmountPage = Math.Ceiling(count / recordsPerPage);
            httpContext.Response.Headers.Add("totalAmountPages", totalAmountPage.ToString());
        }
    }
}
