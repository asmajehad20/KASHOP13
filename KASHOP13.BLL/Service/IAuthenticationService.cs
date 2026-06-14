using KASHOP13.DAL.DTO.Request;
using KASHOP13.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.BLL.Service
{
    public interface IAuthenticationService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<bool> ConfirmEmailAsync(string token, string userId);
        Task<ForgetPasswordResponse> RequestPasswordResetAsync(ForgetPasswordRequest request);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task<LoginResponse> RefreshTokenAsync();
    }
}
