using Dapper;
using Employee.Dtos.Department;
using Employee.Interfaces;
using Employee.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Employee.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ISqlDataAccess _db;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DepartmentRepository(ISqlDataAccess db,IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public  async Task<bool> AddDepartmentAsync(Department dept)
        {
            string query = "DepartmentAdd";
            await _db.SaveData(query, new
            { dept.departmentName, dept.isActive });
            return true;
        }

        public  async Task<bool> DeleteDepartmentAsync(int departmentId)
        {
            string query = "DepartmentDelete";
            await _db.SaveData(query, new { departmentId = departmentId });
            return true;
        }

        public  async Task<PaginateDepartmentDto> GetDepartmentAsync(int page, int limit, string? searchTerm, string? sortColumn, string? sortDirection)
        {
            string query = "DepartmentPaginate";
            using IDbConnection connection = new SqlConnection(_connectionString);
            using var multi = await connection.QueryMultipleAsync(query, new
            {
                Page = page,
                Limit = limit,
                SearchTerm = searchTerm,
                SortColumn = sortColumn,
                SortDirection = sortDirection
            }, commandType: CommandType.StoredProcedure);
            var DepartmentList = await multi.ReadAsync<DepartmentRequestDto>();
            var pData = await multi.ReadFirstAsync<DepartmentPaginationData>();
            return new PaginateDepartmentDto
            {
                List = DepartmentList,
                CountData = pData ?? new DepartmentPaginationData(0, 0)
            };
        }

        public async Task<DepartmentRequestDto?> GetDepartmentByIdAsync(int departmentId)
        {
            string query = "DepartmentGet";
            IEnumerable<DepartmentRequestDto> result = await _db.GetData<DepartmentRequestDto, dynamic>
              (query, new { departmentId = departmentId });

            return result.FirstOrDefault();
        }

        public async Task<DepartmentRequestDto?> GetDepartmentByNameAsync(string departmentName)
        {
            string query = "DepartmentGet";
            IEnumerable<DepartmentRequestDto> result = await _db.GetData<DepartmentRequestDto, dynamic>
              (query, new { departmentName = departmentName });

            return result.FirstOrDefault();
        }

        public  async Task<bool> UpdateDepartmentAsync(Department dept)
        {
            string query = "DepartmentUpdate";
            await _db.SaveData(query, new { dept.departmentId, dept.departmentName,dept.isActive });
            return true;
        }
    }
}
