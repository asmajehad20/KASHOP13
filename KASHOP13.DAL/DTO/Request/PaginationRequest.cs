using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.DAL.DTO.Request
{
    public class PaginationRequest
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string? Search {  get; set; }
    }
}
