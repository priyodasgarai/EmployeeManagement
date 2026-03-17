using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.Models
{
    [Table("Departments")]
    public class Department
    {
        [Key]
        public int departmentId {  get; set; }
 
        public string departmentName { get; set; } = string.Empty;

        [Comment("0=Deactivate,1=Active")]
        public int isActive { get; set; }  

    }
}
