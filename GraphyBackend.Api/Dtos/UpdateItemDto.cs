using System.ComponentModel.DataAnnotations;

namespace GraphyBackend.Api.Dtos
{
	public record UpdateItemDto
	{
		[Required]
		public string Name {get; init;}
		
		[Required]
		[Range(1, 1000)]
		public decimal Price {get; init;}
	}
}

