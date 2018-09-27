using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kaa.ServiceDemo.Service
{
    public class TestService : IHostedService,IDisposable
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
        public Task Doing(CancellationToken cancellationToken)
        {
            return Task.Run(() => {
                _print.Print((n++) + $"TestService Start... cancellationToken.CanBeCanceled:{cancellationToken.CanBeCanceled}");
                while (open && cancellationToken.CanBeCanceled)
                {
                    _print.Print((n++) + $"TestService Doing... cancellationToken.CanBeCanceled:{cancellationToken.CanBeCanceled}");
                    Thread.Sleep(1000);
                }
                _print.Print((n++) + $"TestService Stop... cancellationToken.CanBeCanceled:{cancellationToken.CanBeCanceled}");
            });
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _log.LogInformation("TestService start");
            open = true;
            Doing(cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _log.LogInformation("TestService Stop");
            open = false;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _log.LogInformation("TestService Dispose");
        }
    }
}
