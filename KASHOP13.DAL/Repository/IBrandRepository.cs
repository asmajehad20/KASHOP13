using KASHOP13.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.DAL.Repository
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        Task<List<Brand>> GetAllAsync(string[]? includes = null);
        Task<Brand> CreateAsync(Brand brand);
    }
}
