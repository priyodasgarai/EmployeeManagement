using Employee.Dtos.Designation;
using Employee.Models;

namespace Employee.Interfaces
{
    public interface IDesignationRepository
    {
        Task<bool> AddDesignationAsync(Designation desigDto);
        Task<bool> UpdateDesignationAsync(Designation desigDto);
        Task<bool> DeleteDesignationAsync(int designationId);
        Task<IEnumerable<DesignationRequestDto>> GetDesignationAsync();
        Task<DesignationRequestDto?> GetDesignationByIdAsync(int designationId);
        Task<DesignationRequestDto?> GetDesignationByNameAsync(int departmentId,string designationName);
    }
}
