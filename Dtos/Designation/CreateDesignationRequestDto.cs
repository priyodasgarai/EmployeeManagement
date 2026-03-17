namespace Employee.Dtos.Designation
{
    public class CreateDesignationRequestDto
    {
        public int departmentId { get; set; }

        public string? designationName { get; set; }
    }
}
