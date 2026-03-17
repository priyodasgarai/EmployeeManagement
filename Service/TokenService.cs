using Employee.Interfaces;
using Employee.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Employee.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        public TokenService(IConfiguration config, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _config = config;
            _userManager = userManager;
            _roleManager = roleManager;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Signingkey"]));
        }
        public string CreateToken(AppUser user)
        {
            // var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>{
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
           };
            var cards = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = cards,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
