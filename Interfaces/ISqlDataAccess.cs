namespace Employee.Interfaces
{
    public interface ISqlDataAccess
    {
        Task<IEnumerable<T>> GetData<T, P>(string spName, P parameter, string connectionId = "DefaultConnection");
        Task SaveData<T>(string spName, T parameter, string connectionId = "DefaultConnection");
    }
}
