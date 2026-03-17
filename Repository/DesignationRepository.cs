using Employee.Dtos.Department;
using Employee.Dtos.Designation;
using Employee.Interfaces;
using Employee.Models;

namespace Employee.Repository
{
    public class DesignationRepository : IDesignationRepository
    {
        private readonly ISqlDataAccess _db;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DesignationRepository(ISqlDataAccess db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public  async Task<bool> AddDesignationAsync(Designation desigDto)
        {
            string query = "DesignationAdd";
            await _db.SaveData(query, new
            { desigDto.designationName, desigDto.departmentId });
            return true;
        }

        public  async Task<bool> DeleteDesignationAsync(int designationId)
        {
            string query = "DesignationDelete";
            await _db.SaveData(query, new { designationId = designationId });
            return true;
        }

        public  async Task<IEnumerable<DesignationRequestDto>> GetDesignationAsync()
        {
            string query = "DesignationGet";
            IEnumerable<DesignationRequestDto> result = await _db.GetData<DesignationRequestDto, dynamic>(query, new { });
            return result;
        }

        public async Task<DesignationRequestDto?> GetDesignationByIdAsync(int designationId)
        {
            string query = "DesignationGet";
            IEnumerable<DesignationRequestDto> result = await _db.GetData<DesignationRequestDto, dynamic>
              (query, new { designationId = designationId });
            return result.FirstOrDefault();
        }

        public async Task<DesignationRequestDto?> GetDesignationByNameAsync(int departmentId, string designationName)
        {
            string query = "DesignationGet";
            IEnumerable<DesignationRequestDto> result = await _db.GetData<DesignationRequestDto, dynamic>
              (query, new { designationName = designationName, departmentId= departmentId });
            return result.FirstOrDefault();
        }

        public  async Task<bool> UpdateDesignationAsync(Designation desigDto)
        {
            string query = "DesignationUpdate";
            await _db.SaveData(query, new { desigDto.departmentId, desigDto.designationName, desigDto.designationId });
            return true;
        }
    }
}
