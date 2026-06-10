using KASHOP13.DAL.Data;
using KASHOP13.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace KASHOP13.DAL.Repository
{
    public class CategoryRepository : GenericRepository<Category>,ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context):base(context) { }
        //private readonly ApplicationDbContext _context;

        //public CategoryRepository(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        //public async Task<Category> CreateAsync(Category category)
        //{
        //    await _context.AddAsync(category);
        //    await _context.SaveChangesAsync();
        //    return category;
        //}

        //public async Task<List<Category>> GetAllAsync()
        //{
        //    return await _context.Categories.Include(c => c.Translations).ToListAsync();
            
        //}
    }
}
