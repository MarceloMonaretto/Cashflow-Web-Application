using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Microsoft.Data.Sqlite;
using Npgsql;

namespace EditorNetCoreDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Register the factory
            // In a "real" project you would only use one of these - but the demo package for
            // Editor supports multiple database types.
            var dbType = Environment.GetEnvironmentVariable("DBTYPE");
            if ( dbType == "sqlserver" ) {
                DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);
            }
            else if ( dbType == "mysql" ) {
                DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySqlClientFactory.Instance);
            }
            else if ( dbType == "postgres" ) {
                DbProviderFactories.RegisterFactory("Npgsql", NpgsqlFactory.Instance);
            }
            else if ( dbType == "sqlite" ) {
                DbProviderFactories.RegisterFactory("Microsoft.Data.Sqlite", SqliteFactory.Instance);
            }

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
