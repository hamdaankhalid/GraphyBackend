using Moq;
using Xunit;
using FluentAssertions;
using GraphyBackend.Api.Repositories;
using System;
using GraphyBackend.Api.Models;
using System.Threading.Tasks;
using GraphyBackend.Api.Dtos;
using GraphyBackend.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Queues;
using Azure;
using Azure.Storage.Queues.Models;

namespace GraphyBackend.UnitTests
{

    public class ItemsControllerTests
    {

        private readonly Mock<IItemsRepository> repositoryStub = new Mock<IItemsRepository>();
		private readonly Mock<QueueClient> queueClient = new Mock<QueueClient>(); // TODO: Need to add the package for this
        private readonly Random rand = new Random();

        [Fact]
        public async Task GetItem_WithUnexistingItem_ReturnsNotFound()
        {
            // Arrange
			repositoryStub.Setup(repo => repo.GetItem(It.IsAny<Guid>())).ReturnsAsync((Item)null);
            var controller = new ItemsController(repositoryStub.Object, queueClient.Object);

            // Act

            var result = await controller.GetItem(Guid.NewGuid());

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetItem_WithExistingItem_ReturnsExpectedItem()
        {
            // Arrange
            var expectedItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItem(It.IsAny<Guid>())).ReturnsAsync(expectedItem);
            var controller = new ItemsController(repositoryStub.Object, queueClient.Object);

            // Act

            var result = await controller.GetItem(Guid.NewGuid());

            // Assert
            result.Value.Should().BeEquivalentTo(
                    expectedItem,
                    options => options.ComparingByMembers<Item>()
            );
        }

		[Fact]
		public async Task GetItems_WithExistingItems_ReturnsExpectedItems()
		{
			var expectedItems = new Item[3]{CreateRandomItem(), CreateRandomItem(), CreateRandomItem()};

			repositoryStub.Setup(repo => repo.GetItems()).ReturnsAsync(expectedItems);
			var controller = new ItemsController(repositoryStub.Object, queueClient.Object);

			var result = await controller.GetItems();

			result.Should().BeEquivalentTo(
					expectedItems,
					options => options.ComparingByMembers<Item>()
			);
		}

		[Fact]
		public async Task GetItems_WhenNoExistingItems_ReturnsEmptyList()
		{
		
			var expectedItems = new Item[]{};
			repositoryStub.Setup(repo => repo.GetItems()).ReturnsAsync(expectedItems);
			var controller = new ItemsController(repositoryStub.Object, queueClient.Object);

			var result = await controller.GetItems();

			result.Should().BeEquivalentTo(
					expectedItems,
					options => options.ComparingByMembers<Item>()
			);
		}

		[Fact]
		public async Task CreateItem_WithItemToCreate_ReturnsCreatedItem()
		{
			var queueClientResponse = new Response<SendReceipt>();
			queueClient.Setup(q => q.SendMessageAsync(It.IsAny<String>())).ReturnsAsync(queueClientResponse);

			var createItemRequest = new CreateItemDto() {
				Name = Guid.NewGuid().ToString(),
				Price = rand.Next(1000)
			};

			var controller = new ItemsController(repositoryStub.Object, queueClient.Object);

			var result = await controller.CreateItem(createItemRequest);

			var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;

			result.Should().BeEquivalentTo(
				createdItem,
				options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers()
			);
			
			createdItem.Id.Should().NotBeEmpty();
		}

		[Fact]
		public async Task UpdateItem_WithExistingItem_ReturnsNoContent()
		{
			var existingItem = CreateRandomItem();
			repositoryStub.Setup(repo => repo.GetItem(It.IsAny<Guid>()))
				.ReturnsAsync(existingItem);
	
			var itemId = existingItem.Id;
			var itemToUpdate = new UpdateItemDto { Name="hamdaan", Price = existingItem.Price + 3 };
			
			var controller = new ItemsController(repositoryStub.Object, queueClient.Object);
			var result = await controller.UpdateItem(itemId, itemToUpdate);
			result.Should().BeOfType<NoContentResult>();
		}

		[Fact]
		public async Task DeleteItem_WithExistingItem_ReturnsNoContent()
		{
			var existingItem = CreateRandomItem();
			repositoryStub.Setup(repo => repo.GetItem(It.IsAny<Guid>()))
				.ReturnsAsync(existingItem);
	
			var itemId = existingItem.Id;
			
			var controller = new ItemsController(repositoryStub.Object, queueClient.Object);
			var result = await controller.DeleteItem(itemId);
			result.Should().BeOfType<NoContentResult>();
		}



        private Item CreateRandomItem()
        {
            return new Item
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Price = rand.Next(1000),
                CreatedDate = DateTimeOffset.UtcNow
            };
        }
    }

}
