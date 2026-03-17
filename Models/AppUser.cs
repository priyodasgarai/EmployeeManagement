using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Employee.Models
{
    public class AppUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? ProfilePicture { get; set; }
        public EmployeeModel? employeeModel { get; set; }
        //public List<Address> Addresses { get; set; } = new List<Address>();
        //public List<Order> Orders { get; set; } = new List<Order>();
        //public List<Cart> Carts { get; set; } = new List<Cart>();

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

    }
}
