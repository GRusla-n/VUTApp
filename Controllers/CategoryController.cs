using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using VUTApp.Dtos;
using VUTApp.Models;

namespace VUTApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<List<CategoryDto>> GetAllCategory()
        {
            var category = await _context.Categories.ToListAsync();
            var categoryDto = _mapper.Map <List<CategoryDto>>(category);            
            return categoryDto;
        }

        [HttpGet("{Id}", Name = "getCategory")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int Id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == Id);

            if(category == null)
            {
                return NotFound();
            }

            var categoryDto = _mapper.Map<CategoryDto>(category);

            return categoryDto;
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory(CategoryCreateDto categoryCreation)
        {
            var category = _mapper.Map<Category>(categoryCreation);
            _context.Add(category);
            await _context.SaveChangesAsync();
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return new CreatedAtRouteResult("getCategory", new { categoryDto.Id }, categoryDto);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<Category>> DeleteCategory(int Id)
        {
            var category = await _context.Categories.AnyAsync(x => x.Id == Id);
            if (!category)
            {
                return NotFound();
            }

            _context.Remove(new Category() { Id = Id });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch]
        public ActionResult<Category> EditCategory()
        {
            return NotFound();
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult<Category>> EditCategory(int Id, [FromBody] CategoryCreateDto categoryCreation)
        {
            var category = _mapper.Map<Category>(categoryCreation);
            category.Id = Id;
            _context.Entry(category).State = EntityState.Modified;    
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
