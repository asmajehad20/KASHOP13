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
        Task<PaginationResponse<ProductResponse>> GetAllProductsAsync(PaginationRequest request);
        Task<ProductResponse?> GetProduct(Expression<Func<Product, bool>> filter);
        Task<bool> DeleteProduct(int id);
        Task<bool> UpdateProduct(int id, ProductUpdateRequest request);
        Task<bool> ToggleStatus(int id);
    }
}
