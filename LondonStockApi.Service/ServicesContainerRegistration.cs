using Autofac;
using LondonStockApi.Service.Interfaces;
using LondonStockAPI.DataAccess.Dapper;
using LondonStockAPI.Models;

namespace LondonStockApi.Service
{
    public class ServicesContainerRegistration
    {
        public static void Register(ContainerBuilder builder, DatabaseConfiguration databaseConfiguration)
        {
            builder.RegisterType<LondonStockService>().As<ILondonStockService>();

            DataAccessContainerRegistration.Register(builder, databaseConfiguration);
        }
    }
}
