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
        ChatikServer server;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var server = new ChatikServer(12345);

            try
            {
                cancellationToken.Register(() => server.Stop());
                return server.StartAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                //server.Stop();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
