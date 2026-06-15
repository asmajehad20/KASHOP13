using KASHOP13.DAL.DTO.Request;
using KASHOP13.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.DAL.DTO.Response
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PhoneNumber { get; set; }
        public decimal AmountPaid { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public DateTime OrderDate { get; set; }
        //public List<OrderItemResponse> OrderItems { get; set; }
    }
}
