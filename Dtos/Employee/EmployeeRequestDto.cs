using System.ComponentModel.DataAnnotations;

namespace Employee.Dtos.Employee
{
    public class EmployeeRequestDto
    {
        public int employeeId { get; set; }
        public string? AppUserId { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
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
    }
}
