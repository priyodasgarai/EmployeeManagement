using Employee.Dtos.Department;
using Employee.Dtos.Employee;
using Employee.Models;
namespace Employee.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<bool> AddEmployeeAsync(EmployeeModel emp);
        Task<bool> UpdateEmployeeAsync(EmployeeModel emp);
        Task<bool> DeleteEmployeeAsync(int employeeId);
        Task<EmployeeRequestDto?> GetEmployeeAsync();
        Task<EmployeeRequestDto?> GetEmployeeByIdAsync(int employeeId);
        Task<EmployeeRequestDto?> GetEmployeeByUserAsync(string AppUserId);
        Task<PaginateEmployeeDto> GetEmployeeAsync(int page, int limit, string? searchTerm, string? sortColumn, string? sortDirection);
    }
}
