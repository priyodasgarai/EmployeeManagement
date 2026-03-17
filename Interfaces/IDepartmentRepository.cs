using Employee.Dtos.Department;
using Employee.Models;

namespace Employee.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<bool> AddDepartmentAsync(Department dept);
        Task<bool> UpdateDepartmentAsync(Department dept);
        Task<bool> DeleteDepartmentAsync(int departmentId); 
        Task<DepartmentRequestDto?> GetDepartmentByIdAsync(int departmentId);
        Task<DepartmentRequestDto?> GetDepartmentByNameAsync(string departmentName);
        Task<PaginateDepartmentDto> GetDepartmentAsync(int page, int limit, string? searchTerm, string? sortColumn, string? sortDirection);
    }
}
