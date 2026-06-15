using KASHOP13.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.BLL.Service
{
    public interface IUserManagementService
    {
        Task<List<UserListResponse>> GetAllUsers();
        Task<UserDetailsResponse?> GetUser(string userId);
        Task<bool> ChangeRole(string userId, string role);
        Task<bool> ToggleBlockUser(string userId);
        Task<bool> DeleteUser(string userId);
    }
}
