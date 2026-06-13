using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.DAL.DTO.Request
{
    public class AddToCartRequest
    {
        public int ProductId { get; set; }
        public int Count { get; set; } = 1;
    }
}
