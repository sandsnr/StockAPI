using Autofac;
using LondonStockAPI.DataAccess.Dapper.Interfaces;
using LondonStockAPI.Models;

namespace LondonStockAPI.DataAccess.Dapper
{
    public class DataAccessContainerRegistration
    {
        public static void Register(ContainerBuilder builder, DatabaseConfiguration databaseConfiguration)
        {
            builder.RegisterType<DatabaseConnectionFactory>().As<IDatabaseConnectionFactory>();
            builder.RegisterType<LondonStockRepository>().As<ILondonStockRepository>().WithParameter("databaseConfiguration", databaseConfiguration);
        }
    }
}
