using KASHOP13.DAL.DTO.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.DAL.Models
{
    public enum OrderStatusEnum
    {
        Pending = 1,
        Approved = 2,
        Shipped = 3,
        Delivered = 4,
        Canceled = 5,
        Paid = 6
    }
    public class Order
    {
        public int Id { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? ShippedDate {  get; set; }
        public OrderStatusEnum OrderStatus { get; set; } = OrderStatusEnum.Pending;
        public string? StripeSessionId { get; set; }
        public decimal? AmountPaid { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PhoneNumber { get; set; }

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
