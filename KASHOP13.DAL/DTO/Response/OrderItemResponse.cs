using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.DAL.DTO.Response
{
    public class OrderItemResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
