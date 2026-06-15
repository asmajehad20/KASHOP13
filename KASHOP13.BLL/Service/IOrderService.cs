using KASHOP13.DAL.DTO.Request;
using KASHOP13.DAL.DTO.Response;
using KASHOP13.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.BLL.Service
{
    public interface IOrderService
    {
        Task<List<OrderResponse>> GetUserOrder(string userId);
        Task<OrderDetailsResponse?> GetUserOrder(string userId, int orderId);
        Task<bool> CancelOrder(string userId, int orderId);

        //for admin
        Task<List<OrderResponse>> GetAllOrders(OrderStatusEnum status);
        Task<bool> ChangeOrderStatus(int orderId, ChangeOrderStatusRequest request);
    }
}
