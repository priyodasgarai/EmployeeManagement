using Employee.Dtos.Department;

namespace Employee.Dtos.Employee
{
    public class PaginateEmployeeDto
    {
        public IEnumerable<EmployeeRequestDto> List { get; set; }
        public EmployeePaginationData CountData { get; set; }
    }
    public record EmployeePaginationData(int TotalRecords, int TotalPages);
}
