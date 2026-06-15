using KASHOP13.DAL.DTO.Request;
using KASHOP13.DAL.Models;
using KASHOP13.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.BLL.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IReviewRepository _reviewRepository;
        public ReviewService(IOrderRepository orderRepository, IReviewRepository reviewRepository)
        {
            _orderRepository = orderRepository;
            _reviewRepository = reviewRepository;
        }
        public async Task<bool> AddReview(string userId, AddReviewRequest request)
        {
            var purchasedOrder = await _orderRepository.GetOne(
                filter: o => o.UserId == userId &&
                o.OrderStatus == OrderStatusEnum.Delivered &&
                o.Items.Any(oi => oi.ProductId == request.ProductId),
                includes: new[]
                {
                    nameof(Order.Items)
                });

            if(purchasedOrder == null) return false;

            var AlreadyReviwed = await _reviewRepository.GetOne(
                r => r.UserId == userId && r.ProductId == request.ProductId
                );

            if(AlreadyReviwed != null ) return false;
            var review = request.Adapt<Review>();
            review.UserId = userId;

            await _reviewRepository.CreateAsync(review);
            return true;
        }
    }
}
