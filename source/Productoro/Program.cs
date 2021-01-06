using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Productoro.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Optional;

namespace Productoro
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Thread.CurrentThread.Name = "Main";
            const string DevelopmentEnvironment = "Development";
            const string ProductionEnvironment = "Production";

            var aspNetCoreEnvironment = Environment
                .GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                .AsOption();

            var dotnetEnvironment = Environment
                .GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                .AsOption();

            var debugEnvironment = Debugger.IsAttached
                ? Option.Some(DevelopmentEnvironment)
                : Option.None<string>();

            var environment = aspNetCoreEnvironment
                .Else(dotnetEnvironment)
                .Else(debugEnvironment)
                .Or(ProductionEnvironment);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(ApplicationPath.Directory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .PipeTo(builder => environment
                    .Match(
                        some: env => builder.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true),
                        none: () => builder))
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            using var host = Host
                .CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging
                        .AddConfiguration(configuration)
                        .AddConsole()
                        .AddDebug();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                    config.AddConfiguration(configuration))
                .ConfigureWebHostDefaults(webBuilder => {
                    var port = configuration["Host:Port"];
                    webBuilder
                        .UseContentRoot(ApplicationPath.Directory)
                        .UseKestrel()
                        .UseUrls($"http://+:{port}")
                        .UseStartup<Startup>();
                })
                .Build();
            
            using var cancellationTokenSource = new CancellationTokenSource();
            var runTask = host.RunAsync(cancellationTokenSource.Token);

            await Console.In.ReadLineAsync().ConfigureAwait(false);
            cancellationTokenSource.Cancel();

            await runTask.ConfigureAwait(false);
        }
    }
}
