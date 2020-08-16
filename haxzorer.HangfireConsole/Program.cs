using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace haxzorer.HangfireConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // Configure logging
            var loggingConfig = config.GetSection("Logging");

            ConfigureSerilog(loggingConfig);

            CreateHostBuilder(args).Build().Run();
        }

        private static void ConfigureSerilog(IConfigurationSection loggingConfig)
        {
            var config = new LoggerConfiguration();

            config
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Logger(lc => lc.WriteTo.Console())
                .WriteTo.File("logs/log.txt");

            Serilog.Debugging.SelfLog.Enable(Console.Error);

            Log.Logger = config.CreateLogger();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
