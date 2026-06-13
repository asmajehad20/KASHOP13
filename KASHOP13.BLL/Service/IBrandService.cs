using KASHOP13.DAL.DTO.Request;
using KASHOP13.DAL.DTO.Response;
using KASHOP13.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace KASHOP13.BLL.Service
{
    public interface IBrandService
    {
        Task<List<BrandResponse>> GetAllBrands();
        Task CreateBrand(BrandRequest request);
        Task<BrandResponse?> GetBrand(Expression<Func<Brand, bool>> filter);
        Task<bool> DeleteBrand(int id);
    }
}
