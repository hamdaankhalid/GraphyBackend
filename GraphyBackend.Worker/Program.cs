using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GraphyBackend.Worker.QueueClients;

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
					services.AddSingleton<ItemCreatedQueueClient>(sp => 
					{ 
						return new ItemCreatedQueueClient(azureStorageQueueConnectionString); 
					});
					services.AddHostedService<ItemProcessorWorker>();
                });
    }
}
