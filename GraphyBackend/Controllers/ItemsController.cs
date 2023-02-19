using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using GraphyBackend.Models;
using GraphyBackend.Repositories;

namespace GraphyBackend.Controllers 
{
	[ApiController]
	[Route("items")] // whatever the name of the controller is that will be the route
	public class ItemsController : ControllerBase
	{
		private readonly InMemItemsRepository repository;
			
		public ItemsController(private readonly InMemItemsRepository repository) 
		{
				
		}
		
		// GET /items
		[HttpGet]
		public IEnumerable<Item> GetItems()
		{
			var items = repository.GetItems();
			return items;
		}
		
		// GET /items/id
		[HttpGet("{id}")]
		public ActionResult<Item> GetItem(Guid id)
		{
			var item = repository.GetItem(id);
			
			if (item is null)
			{
				return NotFound();
			}

			return item;
		}
	}
}

