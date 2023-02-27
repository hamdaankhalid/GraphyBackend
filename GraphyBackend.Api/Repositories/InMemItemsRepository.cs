using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphyBackend.Api.Models;

namespace GraphyBackend.Api.Repositories
{
       public class InMemItemsRepository : IItemsRepository
	{
		private readonly List<Item> items = new List<Item>{
			new Item { Id = Guid.NewGuid(), Name = "Potion", Price = 9, CreatedDate = DateTimeOffset.UtcNow },
			new Item { Id = Guid.NewGuid(), Name = "Iron Sword", Price = 20, CreatedDate = DateTimeOffset.UtcNow },
			new Item { Id = Guid.NewGuid(), Name = "Bronze Shield", Price = 18, CreatedDate= DateTimeOffset.UtcNow }
		};
		
		public async Task<IEnumerable<Item>> GetItems()
		{
			return await Task.FromResult(items);
		}

		public async Task<Item> GetItem(Guid id)
		{
			// return item or return null
			return await Task.FromResult(items.Where(item => item.Id == id).SingleOrDefault());
		}
	
		
		public async Task CreateItem(Item item)
		{
			items.Add(item);
			await Task.CompletedTask;
		}
		
		public async Task UpdateItem(Item item)
		{
			var index = items.FindIndex(existingItem => existingItem.Id == item.Id);
			items[index] = item;
			await Task.CompletedTask;
		}

		public async Task DeleteItem(Guid id)
		{
			var index = items.FindIndex(exstItem => exstItem.Id == id);
			items.RemoveAt(index);
			await Task.CompletedTask;
		}
	}
}

