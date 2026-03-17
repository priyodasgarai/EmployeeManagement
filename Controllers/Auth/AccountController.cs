using CoreApiResponse;
using Employee.Dtos.Auth;
using Employee.Helpers;
using Employee.Interfaces;
using Employee.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Employee.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IConfiguration configuration,
            RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager,
            ITokenService tokenService,
            ILogger<AccountController> logger)

        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _logger = logger;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userNameExists = await _userManager.FindByNameAsync(model.Username);
            if (userNameExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Message = "User name already exists!" });


            var userEmailExists = await _userManager.FindByEmailAsync(model.Email);
            if (userEmailExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Message = "User email already exists!" });
                // return CustomResult("Invalid Email Id ", HttpStatusCode.BadRequest);
            }
            var appUser = new AppUser
            {
                UserName = model.Username,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var result = await _userManager.CreateAsync(appUser, model.Password);
            if (!result.Succeeded)
            {
                return CustomResult("User registered faild", result.Errors, HttpStatusCode.BadRequest);

            }
            return CustomResult("User registered Successfully", HttpStatusCode.OK);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var user = await _userManager.FindByEmailAsync(model.Email);
                // return Ok(user);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var authClaims = new List<Claim>{
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(JwtRegisteredClaimNames.GivenName,user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                };
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }


                    // authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Signingkey"]));
                    var token = new JwtSecurityToken(
                                       issuer: _configuration["JWT:Issuer"],
                                       audience: _configuration["JWT:Audience"],
                                       expires: DateTime.Now.AddHours(3),
                                       claims: authClaims,
                                       signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                                   );
                    var newUser = new
                    {
                        Name = user.Name,
                        PhonNo = user.PhoneNumber,
                        UserName = user.UserName,
                        Email = user.Email,
                        UserId = user.Id,
                        Roles = userRoles,
                        Token = new JwtSecurityTokenHandler().WriteToken(token)
                    };
                    //var newUser = new NewUserDto
                    //{
                    //    UserName = user.UserName,
                    //    Email = user.Email,
                    //    // Roles=userRoles.ToList().,
                    //    Token = new JwtSecurityTokenHandler().WriteToken(token)
                    //};
                    //return StatusCode(StatusCodes.Status200OK, new Response {Status=true, Message = "User login Successfully!",Data = newUser});
                    return CustomResult("User login Successfully", newUser, HttpStatusCode.OK);
                }
                return CustomResult("Invalid username", HttpStatusCode.BadRequest);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }

        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return CustomResult("User Not found", HttpStatusCode.NotFound);
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return CustomResult("User delete Successfully", HttpStatusCode.OK);
                }
                else
                {
                    return CustomResult("User delete faild", HttpStatusCode.NotFound);
                }
            }
        }
        [HttpGet("all-user")]

        public async Task<IActionResult> allUser()
        {
            try
            {
                var users = _userManager.Users.ToList(); // Retrieve all users
                var usersWithRoles = new List<UserRolesViewModel>();
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user); // Asynchronously get roles for each user
                    usersWithRoles.Add(new UserRolesViewModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Roles = roles
                    });
                }               
                if (usersWithRoles == null)
                {
                    return CustomResult("Data not found", HttpStatusCode.NotFound);
                }

                return CustomResult("Data loaded successfully", usersWithRoles, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpGet()]
        [Route("userList")]
        public async Task<IActionResult> UserList(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Ensure pageNumber is at least 1
                pageNumber = pageNumber < 1 ? 1 : pageNumber;

                // Get the queryable source
                var usersQuery = _userManager.Users.OrderBy(u => u.UserName);
                var totalUsers = await usersQuery.CountAsync();
                // Apply pagination
                var users = await usersQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var usersWithRoles = new List<UserRolesViewModel>();
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user); // Asynchronously get roles for each user
                    usersWithRoles.Add(new UserRolesViewModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Roles = roles
                    });
                }
                var userData = new
                {
                    count = totalUsers,
                    userList = usersWithRoles
                };
                return CustomResult("Data loaded successfully", userData, HttpStatusCode.OK);
               
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
               
            }
        }




    }
}
