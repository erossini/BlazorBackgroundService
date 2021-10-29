using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BlazorBackgroundService.BlazorUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));
            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule!.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                Directory.SetCurrentDirectory(pathToContentRoot!); 
            }

            var builder = CreateHostBuilder(args.Where(arg => arg != "--console").ToArray());
            
            if (isService)
            {
                if (OperatingSystem.IsWindows())
                    builder.UseWindowsService();
                else if (OperatingSystem.IsLinux())
                    builder.UseSystemd();
                else
                    throw new InvalidOperationException(
                        $"Can not run this application as a service on this Operating System");
            }

            builder.Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, configuration) => configuration
                    .WriteTo.Console()
                    .ReadFrom.Configuration(context.Configuration))
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
