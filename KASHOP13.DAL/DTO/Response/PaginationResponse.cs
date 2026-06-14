using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.DAL.DTO.Response
{
    public class PaginationResponse<T>
    {
        public List<T> Data { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; } = 0;
        public int Limit { get; set; } = 10;

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / Limit);
    }
}
