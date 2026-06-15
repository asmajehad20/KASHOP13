using KASHOP13.DAL.DTO.Request;
using KASHOP13.DAL.DTO.Response;
using KASHOP13.DAL.Models;
using KASHOP13.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.BLL.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository) 
        { 
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderResponse>> GetUserOrder(string userId)
        {
            var orders = await _orderRepository.GetAllAsync(
                filter: o=>o.UserId == userId,
                includes: new[]
                {
                    nameof(Order.Items),
                    $"{nameof(Order.Items)}.{nameof(OrderItem.Product)}",
                    $"{nameof(Order.Items)}.{nameof(OrderItem.Product)}.{nameof(Product.Translations)}"
                } );

            return orders.Adapt<List<OrderResponse>>();
        }

        public async Task<OrderDetailsResponse?> GetUserOrder(string userId, int orderId)
        {
            var order = await _orderRepository.GetOne(
                filter: o => o.UserId == userId && o.Id == orderId,
                includes: new[]
                {
                    nameof(Order.Items),
                    $"{nameof(Order.Items)}.{nameof(OrderItem.Product)}",
                    $"{nameof(Order.Items)}.{nameof(OrderItem.Product)}.{nameof(Product.Translations)}"
                });

            if (order == null) return null;
            return order.Adapt<OrderDetailsResponse>();
        }

        public async Task<bool> CancelOrder(string userId, int orderId)
        {
            var order = await _orderRepository.GetOne(
                filter: o => o.UserId == userId && o.Id == orderId
                );

            if (order is null) return false;
            if (order.OrderStatus != OrderStatusEnum.Pending) return false;

            order.OrderStatus = OrderStatusEnum.Canceled;

            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<List<OrderResponse>> GetAllOrders(OrderStatusEnum status)
        {
            var orders = await _orderRepository.GetAllAsync(
                filter: o=>o.OrderStatus == status);

            return orders.Adapt<List<OrderResponse>>();
        }

        public async Task<bool> ChangeOrderStatus(int orderId, ChangeOrderStatusRequest request)
        {
            var order = await _orderRepository.GetOne(o=>o.Id == orderId);

            if(order.OrderStatus == OrderStatusEnum.Canceled || order.OrderStatus == OrderStatusEnum.Delivered) return false;

            if((int)request.Status != (int)order.OrderStatus + 1) return false;
            order.OrderStatus = request.Status;
            return await _orderRepository.UpdateAsync(order);
        }
    }
}
