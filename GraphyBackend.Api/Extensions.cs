using GraphyBackend.Api.Dtos;
using GraphyBackend.Api.Models;

namespace GraphyBackend.Api
{
       public static class Extensions
	{
		public static ItemDto AsDto(this Item item)
		{
			return new ItemDto
			{
				Id = item.Id,
				Name = item.Name,
				Price = item.Price,
				CreatedDate = item.CreatedDate
			};
		}
	}
}
