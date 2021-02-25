using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using VUTApp.Models;

namespace VUTApp
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
