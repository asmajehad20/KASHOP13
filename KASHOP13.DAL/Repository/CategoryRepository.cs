using KASHOP13.DAL.Data;
using KASHOP13.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace KASHOP13.DAL.Repository
{
    public class CategoryRepository : GenericRepository<Category>,ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context):base(context) { }
        
    }
}
