using Employee.Dtos.Auth;
using Employee.Interfaces;
using Employee.Models;
using Microsoft.AspNetCore.Identity;

namespace Employee.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        public RoleRepository(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<List<string>> AddRolesAsync(string[] roles)
        {
            var rolesList = new List<string>();
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                    rolesList.Add(role);
                }
            }
            return rolesList;
        }
        public async Task<bool> DeleteRolesAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return false;
            }
            await _roleManager.DeleteAsync(role);
            return true;
        }

        public async Task<bool> AddUserRoleAsync(string urerEmail, string[] roles)
        {
            var user = await _userManager.FindByEmailAsync(urerEmail);
            var exitsRoles = await ExistsRolesAsync(roles);
            if (user != null && exitsRoles.Count == roles.Length)
            {
                var assginRoles = await _userManager.AddToRolesAsync(user, exitsRoles);
                return assginRoles.Succeeded;
            }
            return false;
        }

        private async Task<List<string>> ExistsRolesAsync(string[] roles)
        {
            var rolesList = new List<string>();
            foreach (var role in roles)
            {
                var roleExist = await _roleManager.RoleExistsAsync(role);
                if (roleExist)
                {
                    rolesList.Add(role);
                }
            }
            return rolesList;
        }

        public async Task<List<RoleRequestDto>> GetRolesAsync()
        {
            var roleList = _roleManager.Roles.Select(x =>
            new RoleRequestDto { Id = Guid.Parse(x.Id), Name = x.Name }).ToList();
            return roleList;
        }

        public async Task<List<string>> GetUserRolesAsync(string emailId)
        {
            var user = await _userManager.FindByEmailAsync(emailId);
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.ToList();
        }

        public async Task<bool> DeleteUserRolesAsync(string urerEmail, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(urerEmail);
            if (await _userManager.IsInRoleAsync(user, roleName))
            {
                // 3. Remove the user from the role
                var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    // Optionally update security stamp to immediately invalidate existing cookies
                    await _userManager.UpdateSecurityStampAsync(user);
                    return true;
                }
            }
            return false;
        }

       
    }
}
