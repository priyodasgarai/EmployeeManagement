using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Employee.Dtos.Department
{
    public class CreateDepartmentRequestDto
    {
        [Required, MaxLength(50)]
        public string departmentName { get; set; }      
        public int isActive { get; set; }
    }
}
