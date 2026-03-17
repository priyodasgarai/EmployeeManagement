using CoreApiResponse;
using Employee.Dtos.Auth;
using Employee.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Employee.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : BaseController
    {
        private readonly IRoleRepository _roleRepository;
        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var list = await _roleRepository.GetRolesAsync();
            return CustomResult("Data loaded successfully", list, HttpStatusCode.OK);
        }


        [HttpGet("GetUserRole")]
        public async Task<IActionResult> GetUserRole(string userEmail)
        {
            var userClaims = await _roleRepository.GetUserRolesAsync(userEmail);
            return CustomResult("Data loaded successfully", userClaims, HttpStatusCode.OK);
        }

        [HttpPost("addRoles")]
        public async Task<IActionResult> AddRole(string[] roles)
        {
            var userrole = await _roleRepository.AddRolesAsync(roles);
            if (userrole == null)
            {
                return CustomResult("Role did not found!", HttpStatusCode.BadRequest);
            }
            return CustomResult("Data loaded successfully", userrole, HttpStatusCode.OK);
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteRole([FromRoute] string id)
        {
            var userrole = await _roleRepository.DeleteRolesAsync(id);
            if (userrole)
            {
                return CustomResult("Role delete successfully", HttpStatusCode.OK);
            }
            return CustomResult("Role  not found!", HttpStatusCode.BadRequest);
        }


        [HttpPost("addUserRoles")]
        public async Task<IActionResult> AddUserRole([FromBody] NewUserRoleRequestDto addUser)
        {
            var result = await _roleRepository.AddUserRoleAsync(addUser.UserEmail, addUser.Roles);
            if (!result)
            {
                return CustomResult("Role did not found!", HttpStatusCode.BadRequest);
            }
            return CustomResult("Data added successfully", result, HttpStatusCode.OK);
        }
        [HttpDelete]
        [Route("{UserEmail}/{Role}")]
        public async Task<IActionResult> DeleteUserRole([FromRoute] string UserEmail, String Role)
        {
            var userrole = await _roleRepository.DeleteUserRolesAsync(UserEmail, Role);
            if (userrole)
            {
                return CustomResult("User role delete successfully", HttpStatusCode.OK);
            }
            return CustomResult("Role  not found!", HttpStatusCode.BadRequest);
        }

    }
}
