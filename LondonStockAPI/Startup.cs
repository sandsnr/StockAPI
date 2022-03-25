using Autofac;
using AzureFunctions.Autofac.Configuration;
using LondonStockApi.Service;
using LondonStockAPI.Models;
using Microsoft.Extensions.Logging;
using System;

namespace LondonStockAPI
{
    public class Startup
    {
        public Startup(string functionName)
        {
            DependencyInjection.Initialize(builder =>
            {
                RegisterLogging(builder);

                var dbConfig = GetDatabaseConfig(builder);
                builder.RegisterInstance(dbConfig);

                ServicesContainerRegistration.Register(builder, dbConfig);
            }, functionName);
        }


        private DatabaseConfiguration GetDatabaseConfig(ContainerBuilder builder)
        {
            var connectionString = Environment.GetEnvironmentVariable("LondonStockDatabase");
            return new DatabaseConfiguration { ConnectionString = connectionString };
        }

        private void RegisterLogging(ContainerBuilder builder)
        {
            builder.RegisterInstance(new LoggerFactory()).As<ILoggerFactory>();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();
        }
    }

}