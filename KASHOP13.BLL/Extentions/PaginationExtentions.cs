using KASHOP13.DAL.DTO.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.BLL.Extentions
{
    public static class PaginationExtentions
    {
        public static async Task<PaginationResponse<T>> ToPaginationAsync<T>(this IQueryable<T> query, int page, int limit)
        {
            var totalCount = await query.CountAsync();
            var data = await query.Skip((page-1) *  limit).Take(limit).ToListAsync();

            return new PaginationResponse<T>
            {
                Data = data,
                TotalCount = totalCount,
                Page = page
            };
        }
    }
}
