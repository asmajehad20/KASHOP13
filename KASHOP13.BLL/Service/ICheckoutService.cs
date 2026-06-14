using KASHOP13.DAL.DTO.Request;
using KASHOP13.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.BLL.Service
{
    public interface ICheckoutService
    {
        Task<CheckoutResponse> ProcessCheckout(string userId, CheckoutRequest request);
        Task<CheckoutResponse> HandleSuccess(string sessionId);
    }
}
