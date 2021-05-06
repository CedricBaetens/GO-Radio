using GoRadio.Logic;
using GoRadio.Logic.Database;
using GoRadio.Logic.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace GoRadio.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //// DI
            //var serviceProvider = new ServiceCollection()
            //    .RegisterGoRadio()
            //    .BuildServiceProvider();

            //// Create database
            //var context = serviceProvider.GetRequiredService<DatabaseContext>();
            //context.Database.EnsureCreated();

            //// Get service
            //var microphoneService = serviceProvider.GetRequiredService<AudioDeviceService>();
            //microphoneService.Start();
            //Thread.Sleep(5000);
            //serviceProvider.GetRequiredService<AudioDeviceService>().Play(1);
        }
    }
}
