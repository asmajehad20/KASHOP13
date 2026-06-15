using KASHOP13.DAL.DTO.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.BLL.Service
{
    public interface IReviewService
    {
        Task<bool> AddReview(string userId, AddReviewRequest request);
    }
}
