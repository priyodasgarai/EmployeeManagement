using Employee.Dtos.Auth;

namespace Employee.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<RoleRequestDto>> GetRolesAsync();
        Task<List<string>> GetUserRolesAsync(string emailId);
        Task<List<string>> AddRolesAsync(string[] roles);
        Task<bool> DeleteRolesAsync(string id);
        Task<bool> AddUserRoleAsync(string urerEmail, string[] roles);
        Task<bool> DeleteUserRolesAsync(string urerEmail, string roleName);
    }
}
