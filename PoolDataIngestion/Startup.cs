using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Repository;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(PoolDataIngestion.Startup))]

namespace PoolDataIngestion
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IMyAppSettings>(i =>
            {
                return new MyAppSettings
                {
                    AzureFileConnectionString = Environment.GetEnvironmentVariable("WEBSITE_CONTENTAZUREFILECONNECTIONSTRING")
                };
            });

            builder.Services.AddTransient<IPoolSensorRepository, PoolSensorRepository>();
        }
    }
}
