using KASHOP13.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.DAL.Repository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>?> DecreaseQuantityAsync(List<OrderItem> orderItems);
    }
}
