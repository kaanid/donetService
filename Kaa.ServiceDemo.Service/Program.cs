using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.Loader;

namespace Kaa.ServiceDemo.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, e) => {
                Console.WriteLine("Console.CancelKeyPress");
            };
            AppDomain.CurrentDomain.ProcessExit += (sender, e) => {
                Console.WriteLine("AppDomain.CurrentDomain.ProcessExit");
            };
            AssemblyLoadContext.Default.Unloading += (sender) => {
                Console.WriteLine("AssemblyLoadContext.Default.Unloading");
            };

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

            using (var source = new CancellationTokenSource())
            {
                builder.RunAsync(source.Token);
                //builder.WaitForShutdownAsync(source.Token);
                //builder.RunAsync(source.Token).GetAwaiter().GetResult();

                
                Console.WriteLine("..................");

                //builder.WaitForShutdown();
                //Console.ReadKey();
                source.Cancel();


                Console.ReadKey();
            }
        }


    }
}
