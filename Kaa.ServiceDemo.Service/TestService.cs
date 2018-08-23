using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kaa.ServiceDemo.Service
{
    public class TestService : IHostedService
    {
        private int n = 0;
        private bool open = false;
        private readonly IPrintService _print;
        private readonly ILogger<TestService> _log;

        public TestService(IPrintService print, ILogger<TestService> log)
        {
            _print = print;
            _log = log;
        }
        public async Task Doing(CancellationToken cancellationToken)
        {
            while (open)
            {
                _print.Print((n++) + "TestService Doing...");
                Thread.Sleep(1000);
            }
            await Task.CompletedTask;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _log.LogInformation("TestService start");
            open = true;
            return Doing(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _log.LogInformation("TestService Stop");
            open = false;
            return Task.CompletedTask;
        }
    }
}
