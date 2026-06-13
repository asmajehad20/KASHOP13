using KASHOP13.DAL.DTO.Request;
using KASHOP13.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.BLL.Service
{
    public interface ICartService
    {
        Task<bool> AddToCart(AddToCartRequest request, string UserId);
        Task<List<CartResponse>> GetCart(string userId);
        Task<bool> UpdateQuantity(int productId, int count, string userId);
        Task<bool> RemoveItem(int productId, string userId);
        Task<bool> ClearCart(string userId);
    }
}
