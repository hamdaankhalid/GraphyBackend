using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using GraphyBackend.Dtos;
using GraphyBackend.Repositories;
using GraphyBackend.Models;
using System.Threading.Tasks;

namespace GraphyBackend.Controllers
{
       [ApiController]
	[Route("items")] // whatever the name of the controller is that will be the route
	public class ItemsController : ControllerBase
	{
		private readonly IItemsRepository repository;
			
		public ItemsController(IItemsRepository repository) 
		{
			this.repository = repository;	
		}
		
		// GET /items
		[HttpGet]
		public async Task<IEnumerable<ItemDto>> GetItems()
		{
			var items = (await repository.GetItems()).Select(item => item.AsDto());

			return items;
		}
		
		// GET /items/id
		[HttpGet("{id}")]
		public async Task<ActionResult<ItemDto>> GetItem(Guid id)
		{
			var item = await repository.GetItem(id);
			
			if (item is null)
			{
				return NotFound();
			}

			return item.AsDto();
		}
		
		// POST /items
		[HttpPost]
		public async Task<ActionResult<ItemDto>> CreateItem(CreateItemDto itemDto)
		{
			var item = new Item{
					Id=Guid.NewGuid(),
					CreatedDate= DateTimeOffset.UtcNow,
					Name= itemDto.Name,
					Price= itemDto.Price
				};

			await repository.CreateItem(item);

			return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item.AsDto() );
		}
	
		// GET /items/id
		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateItem(Guid id, UpdateItemDto item)
		{
			var existingItem = await repository.GetItem(id);
			if (existingItem is null)
			{
				return NotFound();
			}

			Item updateItem = existingItem with {
				Name = item.Name,
				Price = item.Price
			};

			await repository.UpdateItem(updateItem);

			return NoContent();
		}
		
		// DELETE /items/id
		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteItem(Guid id)
		{
			var existingItem = await repository.GetItem(id);
			if (existingItem is null)
			{
				return NotFound();
			}
			
			await repository.DeleteItem(id);

			return NoContent();	
		}
	}
}

