using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Productoro.Implementation;
using Productoro.Migrations;
using Productoro.Models;

namespace Productoro
{
    public class Startup
    {
        private static readonly Lazy<string> eventStoreLocation = new Lazy<string>(() =>
        {
            var repositoryLocation = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "Productoro");
            Directory.CreateDirectory(repositoryLocation);
            return repositoryLocation;
        });

        public static string EventStoreLocation => eventStoreLocation.Value;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            var eventStoreDataSource = Path.Combine(EventStoreLocation, "events.db3");
            var connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = eventStoreDataSource,
                Mode = SqliteOpenMode.ReadWriteCreate,
                Cache = SqliteCacheMode.Default,
                ForeignKeys = false
            };

            var databaseConnectionString = new DatabaseConnectionString(connectionString.ToString());

            services
                .AddSingleton<ITimeProvider>(TimeProvider.Instance);

            services
                .AddSingleton<DatabaseConnectionString>(databaseConnectionString)
                .AddFluentMigratorCore()
                .ConfigureRunner(builder => builder
                    .AddSQLite()
                    .WithGlobalConnectionString(databaseConnectionString.Value)
                    .ScanIn(typeof(CreateTables).Assembly).For.Migrations())
                .AddLogging(builder => builder.AddFluentMigratorConsole());

            services
                .AddSingleton<IDatabase, Database>()
                .AddSingleton<IEventStore, SqliteEventStore>();

            services
                .AddSingleton<IEventBus, InMemoryEventBus>();

            services
                .AddTransient<IProjectClient, ProjectClient>()
                .AddTransient<ITaskClient, TaskClient>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var isDevelopment = env.IsDevelopment() || System.Diagnostics.Debugger.IsAttached;
            if (isDevelopment)
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (isDevelopment)
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
