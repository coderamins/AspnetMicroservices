using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.Api.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host,int? retry=0)
        {
            int retryForavailablity=retry.Value;

            using(var scope=host.Services.CreateScope())
            {
                var services=scope.ServiceProvider;
                var configuration=services.GetRequiredService<IConfiguration>();
                var logger=services.GetRequiredService<ILogger<TContext>>();

                try{
                    logger.LogInformation("Migrating postgresql database");

                    using var connection=new NpgsqlConnection
                    (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
    
                    connection.Open();

                    using var command=new NpgsqlCommand{
                        Connection=connection
                    };

                    command.CommandText="DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText=@"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY,
                                          ProductName VARCHAR(24) NOT NULL,
                                          Description TEXT,
                                          Amount INT)";
                    
                    command.ExecuteNonQuery();
                }
            }
        }
        
    }
}