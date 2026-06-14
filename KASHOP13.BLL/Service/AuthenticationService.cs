using KASHOP13.DAL.DTO.Request;
using KASHOP13.DAL.DTO.Response;
using KASHOP13.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KASHOP13.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenticationService(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var user = request.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return new RegisterResponse() 
                { 
                    Success = false, 
                    Message = "Error" ,
                    Errors = result.Errors.Select(p => p.Description).ToList()
                };

            await _userManager.AddToRoleAsync(user, "User");
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = Uri.EscapeDataString(token);
            var emailUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/Account/confirmemail?token={token}&userId={user.Id}";
            await _emailSender.SendEmailAsync(request.Email, "welcome", $"<h2> welcome {user.UserName}</h2>"+
                $"<a href='{emailUrl}'>confirm email</a>");

            return new RegisterResponse() { Success = true, Message = "Success" };
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null) 
                return new LoginResponse() { Success = false, Message = "Invalid email" };
            
            if(!await _userManager.IsEmailConfirmedAsync(user))
            {
                return new LoginResponse() { Success = false, Message = "Email is not confirmed" };
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!result)
            {
                return new LoginResponse()
                {
                    Success = false,
                    Message = "Invalid password"
                };
            }

            var refreshToken = await GenerateRefreshToken(user);
            SetRefreshTokenCookies(refreshToken);


            return new LoginResponse()
            {
                Success = true,
                Message = "Success",
                AccessToken = await GenerateAccessToken(user)
            };
        }

        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(5),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        private async Task<string> GenerateRefreshToken(ApplicationUser user)
        {
            var refreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(15);
            await _userManager.UpdateAsync(user);
            return refreshToken;
        }

        private void SetRefreshTokenCookies(string refreshToken)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,//true for production
                SameSite = SameSiteMode.None,//Strict for production
                Expires = DateTime.UtcNow.AddDays(15)
            });
        }

        public async Task<bool> ConfirmEmailAsync(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return false;

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if(!result.Succeeded) return false;
            return true;
        }

        public async Task<ForgetPasswordResponse> RequestPasswordResetAsync(ForgetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null)
            {
                return new ForgetPasswordResponse()
                {
                    Success = false,
                    Message = "Email not found"
                };
            }

            var random = new Random();
            var code = random.Next(1000, 9999).ToString();

            user.CodeResetPassword = code;
            user.PasswordResetCodeExpiry = DateTime.UtcNow.AddMinutes(20);

            await _userManager.UpdateAsync(user);
            await _emailSender.SendEmailAsync(request.Email, "reset password", $"<p>Code is {code}</p>");

            return new ForgetPasswordResponse()
            {
                Success = true,
                Message = "Success, code sent to your email"
            };
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null)
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "Email not found"
                };
            }
            else if(user.CodeResetPassword != request.Code)
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "code is invalid"
                };
            }
            else if(user.PasswordResetCodeExpiry < DateTime.UtcNow)
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "code expired"
                };
            }

            var isSamePassword = await _userManager.CheckPasswordAsync(user, request.NewPassword);
            if(isSamePassword)
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "new password must be different than the old password"
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            if (!result.Succeeded)
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "Password reset failed"
                };
            }

            await _emailSender.SendEmailAsync(request.Email, "change password", $"<p>your password have been rest successfully</p>");
            
            return new ResetPasswordResponse()
            {
                Success = true,
                Message = "Password reset successfully"
            };
        }

    }
}
