using System.ComponentModel.DataAnnotations;

namespace Employee.Dtos.Employee
{
    public class CreateEmployeeRequestDto
    {

        [Required, MaxLength(10), MinLength(10)]
        public string contactNo { get; set; } 
        [Required]
        public string city { get; set; } 
        public string state { get; set; } 
        public string pincode { get; set; } 
        public string altContactNo { get; set; }
        public string address { get; set; }
        public string employeeRole { get; set; }
        public int designationId { get; set; }
      
    }
}
