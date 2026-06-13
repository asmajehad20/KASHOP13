using KASHOP13.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace KASHOP13.DAL.Repository
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        //Task<List<Category>> GetAllAsync(string[]? includes = null);
        //Task<Category> CreateAsync(Category category);
    }
}
