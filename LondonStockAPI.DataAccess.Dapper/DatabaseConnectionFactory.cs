using LondonStockAPI.DataAccess.Dapper.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace LondonStockAPI.DataAccess.Dapper
{
    public class DatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        public IDbConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
