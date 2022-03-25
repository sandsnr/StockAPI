using System.Data;

namespace LondonStockAPI.DataAccess.Dapper.Interfaces
{
    public interface IDatabaseConnectionFactory
    {
        IDbConnection GetConnection(string connectionString);
    }
}
