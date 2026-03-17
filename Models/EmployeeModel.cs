using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.Models
{
    [Table("Employees")]
    public class EmployeeModel
    {
        [Key]
        public int employeeId { get; set; }
        public string AppUserId { get; set; } = string.Empty;      
        public string contactNo { get; set; } = string.Empty;       
        public string city { get; set; } = string.Empty;
        public string state { get; set; } = string.Empty;
        public string pincode { get; set; } = string.Empty;
        public string altContactNo { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string employeeRole { get; set; } = string.Empty;
        public int designationId { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime updatedDate { get; set; }
        public AppUser? AppUser { get; set; }
        public Designation? Designation { get; set; }
    }
}
