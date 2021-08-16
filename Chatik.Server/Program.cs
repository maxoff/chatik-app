using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Chatik.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Server>();
                })
                .ConfigureLogging(log => 
                {
                    log.AddConsole();
                });
    }
}
