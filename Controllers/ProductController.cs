using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using VUTApp.Dtos;
using VUTApp.Helpers;
using VUTApp.Models;
using VUTApp.Services;
using Microsoft.Extensions.Logging;

namespace VUTApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorageService fileStorageService;
        private readonly ILogger<ProductController> logger;
        private readonly string containerName = "products";

        public ProductController(ApplicationDbContext context, IMapper mapper, IFileStorageService fileStorageService, ILogger<ProductController> logger)
        {
            this.context = context;            
            this.mapper = mapper;
            this.fileStorageService = fileStorageService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetAllProducts([FromQuery] FilterProductDto filterProductDto)
        {
            var productQueryable = context.Products.AsQueryable().Include(u => u.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterProductDto.Name))
            {
                productQueryable = productQueryable.Where(x => x.Name.Contains(filterProductDto.Name));
            }
            if (filterProductDto.MaxPrice != null)
            {
                productQueryable = productQueryable.Where(x => x.Price <= filterProductDto.MaxPrice);
            }
            if (filterProductDto.MinPrice != null)
            {
                productQueryable = productQueryable.Where(x => x.Price >= filterProductDto.MinPrice);
            }            
            if (!string.IsNullOrWhiteSpace(filterProductDto.CategoryName))
            {
                productQueryable = productQueryable.Where(x => x.Category.Name.Contains(filterProductDto.CategoryName));
            }
            if (!string.IsNullOrWhiteSpace(filterProductDto.OrderingField))
            {
                try
                {
                    productQueryable = productQueryable
                    .OrderBy($"{filterProductDto.OrderingField} {(filterProductDto.AscendingOrder ? "ascending" : "descending")}");
                }
                catch
                {
                    logger.LogWarning("Could not order by field: " + filterProductDto.OrderingField);
                }
                
            }

            await HttpContext.InsertPaginationParametersInResponse(productQueryable, filterProductDto.RecordsPerPage);
            var products = await productQueryable.Paginate(filterProductDto).ToListAsync();            
            return mapper.Map<List<ProductDto>>(products);
        }


        [HttpGet("{Id}", Name = "getProduct")]
        public async Task<ActionResult<ProductDto>> GetProductById(int Id)
        {
            var product = await context.Products.Include(u => u.Category).FirstOrDefaultAsync(x => x.Id == Id);
            var productDto = mapper.Map<ProductDto>(product);

            if (product == null)
            {
                return NotFound();
            }

            return productDto;
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromForm] ProductCreateDto productCreationDto)
        {
            var product = mapper.Map<Product>(productCreationDto);

            if (productCreationDto.Image != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await productCreationDto.Image.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(productCreationDto.Image.FileName);
                    product.Image =
                        await fileStorageService.SaveFile(content, extension, containerName, productCreationDto.Image.ContentType);
                }
            }

            context.Add(product);
            await context.SaveChangesAsync();
            var productDto = mapper.Map<ProductDto>(product);
            return new CreatedAtRouteResult("getProduct", new { productDto.Id }, productDto);
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> EditProduct(int id, [FromForm] ProductCreateDto productCreationDto)
        {
            var productDB = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (productDB == null) { return NotFound(); }
            productDB = mapper.Map(productCreationDto, productDB);
            if (productCreationDto.Image != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await productCreationDto.Image.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(productCreationDto.Image.FileName);
                    productDB.Image =
                        await fileStorageService.EditFile(content, extension, containerName, productDB.Image, productCreationDto.Image.ContentType);
                }
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var productDB = await context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (productDB == null)
            {
                return NotFound();
            }

            context.Remove(new Product() { Id = id });
            await context.SaveChangesAsync();
            await fileStorageService.DeleteFile(productDB.Image, containerName);

            return NoContent();
        }        
    }
}
