namespace Employee.Dtos.Department
{
    public class PaginateDepartmentDto
    {
        public IEnumerable<DepartmentRequestDto> List { get; set; }
        public DepartmentPaginationData CountData { get; set; }
    }
    public record DepartmentPaginationData(int TotalRecords, int TotalPages);
}
