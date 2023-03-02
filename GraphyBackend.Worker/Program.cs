using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure.Storage.Queues;

namespace GraphyBackend.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
					var azureStorageQueueConnectionString = Environment.GetEnvironmentVariable("AzureStorageQueueConnectionString");
					var queue = new QueueClient(azureStorageQueueConnectionString, "item-uploaded-queue");
					services.AddSingleton<QueueClient>(sp => { return queue; });
					services.AddHostedService<ItemProcessorWorker>();
                });
    }
}
