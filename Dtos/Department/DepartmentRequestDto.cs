using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Employee.Dtos.Department
{
    public class DepartmentRequestDto
    {
        public int departmentId { get; set; }      

        public string departmentName { get; set; }
       
        public int isActive { get; set; }
    }
}
