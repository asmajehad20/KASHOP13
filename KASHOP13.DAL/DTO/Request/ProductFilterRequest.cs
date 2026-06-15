using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.DAL.DTO.Request
{
    public class ProductFilterRequest : PaginationRequest
    {
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public double? MinRate { get; set; }
    }
}
