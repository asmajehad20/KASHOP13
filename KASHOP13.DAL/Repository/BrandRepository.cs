using KASHOP13.DAL.Data;
using KASHOP13.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.DAL.Repository
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        public BrandRepository(ApplicationDbContext context) : base(context) { }
    }
}
