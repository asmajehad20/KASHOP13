using KASHOP13.DAL.DTO.Request;
using KASHOP13.DAL.DTO.Response;
using KASHOP13.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace KASHOP13.BLL.Service
{
    public interface IProductService
    {
        Task CreateProduct(ProductRequest request);
        Task<List<ProductResponse>> GetAllProductsAsync();
        Task<ProductResponse?> GetProduct(Expression<Func<Product, bool>> filter);
        Task<bool> DeleteProduct(int id);
    }
}
