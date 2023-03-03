using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GraphyBackend.Worker.QueueClients;

namespace GraphyBackend.Worker
{
    public class ItemProcessorWorker : BackgroundService
    {
        private readonly ILogger<ItemProcessorWorker> _logger;
		private readonly ItemCreatedQueueClient queueClient;

        public ItemProcessorWorker(ILogger<ItemProcessorWorker> logger, ItemCreatedQueueClient queueClient)
        {
            _logger = logger;
			this.queueClient = queueClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
			{	
				var msg = await queueClient.ReceiveMessageAsync();
				if (msg.Value != null) 
				{
					Console.WriteLine(msg.Value.Body);
					await queueClient.DeleteMessageAsync(msg.Value.MessageId, msg.Value.PopReceipt);
				}
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
