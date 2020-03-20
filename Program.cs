using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.SystemConsole.Themes;

namespace mywebapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            
            // Console.Write($"Env:{$currnetEnv}");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())                
                .AddJsonFile(path: $"appsettings.{currentEnv}.json", optional: false, reloadOnChange: true)
                // .AddEnvironmentVariables()
                .Build();
        
            //使用從 appsettings.json 讀取到的內容來設定 logger
            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration)
                            .CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging=>{
                    // logging.ClearProviders();
                    // logging.AddConsole();
                    logging.AddSerilog();
                })
                // .UseSerilog((builder, cfg) => 
                // {
                //     cfg.MinimumLevel.Debug()
                //         .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                //         .WriteTo.Console(
                //             theme: AnsiConsoleTheme.Code,
                //             outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Application} {Level:u3}][{RequestId}] {SourceContext}: {Message:lj}{NewLine}{Exception}"
                //         )
                //         .WriteTo.File(
                //             formatter: new CompactJsonFormatter(),
                //             path: "C:/log/myapp/myapp.log",
                //             fileSizeLimitBytes: 10485760,
                //             rollOnFileSizeLimit: true,
                //             retainedFileCountLimit: 3
                //         );
                // })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
