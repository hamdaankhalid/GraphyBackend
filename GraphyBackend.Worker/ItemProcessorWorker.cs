using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Azure.Storage.Queues;

namespace GraphyBackend.Worker
{
    public class ItemProcessorWorker : BackgroundService
    {
        private readonly ILogger<ItemProcessorWorker> _logger;
		private readonly QueueClient queueClient;

        public ItemProcessorWorker(ILogger<ItemProcessorWorker> logger, QueueClient queueClient)
        {
            _logger = logger;
			this.queueClient = queueClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {	
				var msg = await queueClient.ReceiveMessageAsync();
				if (msg.Value != null) {
					Console.WriteLine(msg.Value.Body);
					await queueClient.DeleteMessageAsync(msg.Value.MessageId, msg.Value.PopReceipt);
				}
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
