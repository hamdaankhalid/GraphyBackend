using Azure.Storage.Queues;

namespace GraphyBackend.Worker.QueueClients
{
	public class ItemCreatedQueueClient : QueueClient
	{
		private static readonly string _queueName = "item-uploaded-queue"; 

		public ItemCreatedQueueClient(string azureStorageQueueConnectionString) : 
			base(azureStorageQueueConnectionString, _queueName)
		{
		}
	}
}

