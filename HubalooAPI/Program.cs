using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace HubalooAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            // .ReadFrom.Configuration(configuration)
            // .Enrich.FromLogContext()
            // .Enrich.WithMachineName()
            // .Enrich.WithProcessId()
            // .Enrich.WithThreadId()
            .WriteTo.Console()
            .CreateLogger();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // .ConfigureLogging((context, logging) =>
                // {
                //     logging.ClearProviders();
                //     logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                //     // logging.AddDebug();
                //     logging.AddConsole();
                // })
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
