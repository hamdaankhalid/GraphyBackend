using System;
using System.Collections.Generic;
using System.Linq;
using GraphyBackend.Models;

namespace GraphyBackend.Repositories
{
	public class InMemItemsRepository
	{
		private readonly List<Item> items = new List<Item>{
			new Item { Id = Guid.NewGuid(), Name = "Potion", Price = 9, CreatedDate = DateTimeOffset.UtcNow },
			new Item { Id = Guid.NewGuid(), Name = "Iron Sword", Price = 20, CreatedDate = DateTimeOffset.UtcNow },
			new Item { Id = Guid.NewGuid(), Name = "Bronze Shield", Price = 18, CreatedDate= DateTimeOffset.UtcNow }
		};
		
		public IEnumerable<Item> GetItems()
		{
			return items;
		}

		public Item GetItem(Guid id)
		{
			// return item or return null
			return items.Where(item => item.Id == id).SingleOrDefault();
		}
	}
}
