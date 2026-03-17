using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.Models
{
    [Table("Designations")]
    public class Designation
    {
        [Key]
        public int designationId { get; set; }
        public int departmentId { get; set; }
        
        public string designationName { get; set; } = string.Empty;

        public Department? Department { get; set; } 

    }
}
