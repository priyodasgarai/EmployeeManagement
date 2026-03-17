using Employee.Models;

namespace Employee.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
