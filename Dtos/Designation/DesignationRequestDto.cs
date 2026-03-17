using System.ComponentModel.DataAnnotations;

namespace Employee.Dtos.Designation
{
    public class DesignationRequestDto
    {
        public int designationId { get; set; }
        public int departmentId { get; set; }
        [Required, MaxLength(50)]
        public string? designationName { get; set; }
        public string? departmentName { get; set; }

    }
}
