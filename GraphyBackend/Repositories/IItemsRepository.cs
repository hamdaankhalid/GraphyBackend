using System;
using System.Collections.Generic;
using GraphyBackend.Models;

namespace GraphyBackend.IItemsRepository 
{
	public interface IItemsRepository 
	{
		Item GetItem(Guid id);
		IEnumerable<Item> GetItems();
	}
}

