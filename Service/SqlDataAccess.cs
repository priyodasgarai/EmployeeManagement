using Dapper;
using Employee.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Employee.Service
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _configuration;
        public SqlDataAccess(IConfiguration configuration)
        {

            _configuration = configuration;
        }
        public async Task<IEnumerable<T>> GetData<T, P>(string spName, P parameter, string connectionId = "DefaultConnection")
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString(connectionId));
            return await connection.QueryAsync<T>(spName, parameter, commandType: CommandType.StoredProcedure);
        }
        public async Task SaveData<T>(string spName, T parameter, string connectionId = "DefaultConnection")
        {
            using IDbConnection connection = new SqlConnection(_configuration.GetConnectionString(connectionId));
            await connection.ExecuteAsync(spName, parameter, commandType: CommandType.StoredProcedure);
        }
    }
}
