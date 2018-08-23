using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Kaa.ServiceDemo.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new HostBuilder()
                .UseConsoleLifetime()
                .UseServiceProviderFactory(new AspectCoreServiceProviderFactory())
                .ConfigureAppConfiguration((context, conBuilder) =>
                {
                    conBuilder.AddJsonFile("appsetting.json",true);
                })
                .ConfigureLogging(logging => {
                    logging
                        .AddConsole()
                        .AddDebug();
                })
                .ConfigureServices((context, serviceColl) => {
                    serviceColl.AddSingleton<IPrintService, PrintService>();
                    serviceColl.AddSingleton<IHostedService, TestService>();
                })
                .Build();

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                var logFactory = builder.Services.GetService<ILoggerFactory>();
                var logger = logFactory.CreateLogger<Program>();
                logger.LogInformation(e.ExceptionObject as Exception, $"UnhandledException");
            };

            //builder.Run();

            builder.RunAsync().GetAwaiter().GetResult();

            Console.WriteLine("..................");
            Console.ReadKey();
        }


    }
}
