using Dapper;
using Employee.Dtos.Department;
using Employee.Dtos.Employee;
using Employee.Interfaces;
using Employee.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Employee.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ISqlDataAccess _db;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public EmployeeRepository(ISqlDataAccess db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public  async Task<bool> AddEmployeeAsync(EmployeeModel emp)
        {
            string query = "EmployeesAdd";
            await _db.SaveData(query, new
            { emp.designationId,emp.AppUserId, emp.city, emp.contactNo, emp.state, emp.pincode, emp.address, emp.altContactNo,emp.employeeRole });
            return true;
        }

        public  async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            string query = "EmployeesDelete";
            await _db.SaveData(query, new { employeeId = employeeId });
            return true;
        }

        public  async Task<EmployeeRequestDto?> GetEmployeeAsync()
        {
            throw new NotImplementedException();
        }

        public  async Task<PaginateEmployeeDto> GetEmployeeAsync(int page, int limit, string? searchTerm, string? sortColumn, string? sortDirection)
        {
            string query = "EmployeesPaginate";
            using IDbConnection connection = new SqlConnection(_connectionString);
            using var multi = await connection.QueryMultipleAsync(query, new
            {
                Page = page,
                Limit = limit,
                SearchTerm = searchTerm,
                SortColumn = sortColumn,
                SortDirection = sortDirection
            }, commandType: CommandType.StoredProcedure);
            var EmployeesList = await multi.ReadAsync<EmployeeRequestDto>();
            var pData = await multi.ReadFirstAsync<EmployeePaginationData>();
            return new PaginateEmployeeDto
            {
                List = EmployeesList,
                CountData = pData ?? new EmployeePaginationData(0, 0)
            };
        }

        public  async Task<EmployeeRequestDto?> GetEmployeeByIdAsync(int employeeId)
        {
            string query = "EmployeesGet";
            IEnumerable<EmployeeRequestDto> result = await _db.GetData<EmployeeRequestDto, dynamic>
              (query, new { employeeId = employeeId });

            return result.FirstOrDefault();
        }

        public async Task<EmployeeRequestDto?> GetEmployeeByUserAsync(string AppUserId)
        {
            string query = "EmployeesGet";
            IEnumerable<EmployeeRequestDto> result = await _db.GetData<EmployeeRequestDto, dynamic>
              (query, new { AppUserId = AppUserId });

            return result.FirstOrDefault();
        }

        public  async Task<bool> UpdateEmployeeAsync(EmployeeModel emp)
        {
            string query = "EmployeesUpdate";
            await _db.SaveData(query, new { emp.designationId, emp.employeeId, emp.city, emp.contactNo, emp.state, emp.pincode, emp.address, emp.altContactNo, emp.employeeRole });
            return true;
        }
    }
}
