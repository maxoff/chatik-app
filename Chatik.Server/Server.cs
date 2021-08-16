using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Chatik.Server
{
    public class Server : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var server = new ChatikServer(12345);

                cancellationToken.Register(() => server.Stop());
                var t = server.StartAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
