using Moq;
using Xunit;
using FluentAssertions;
using GraphyBackend.Api.Repositories;
using System;
using GraphyBackend.Api.Models;
using System.Threading.Tasks;
using GraphyBackend.Api.Controllers;
using Microsoft.AspNetCore.Mvc;


namespace GraphyBackend.UnitTests
{

    public class ItemsControllerTests
    {

        private readonly Mock<IItemsRepository> repositoryStub = new Mock<IItemsRepository>();

        private readonly Random rand = new Random();

        [Fact]
        public async Task GetItem_WithUnexistingItem_ReturnsNotFound()
        {
            // Arrange
			repositoryStub.Setup(repo => repo.GetItem(It.IsAny<Guid>())).ReturnsAsync((Item)null);
            var controller = new ItemsController(repositoryStub.Object);

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
            var controller = new ItemsController(repositoryStub.Object);

            // Act

            var result = await controller.GetItem(Guid.NewGuid());

            // Assert
            result.Value.Should().BeEquivalentTo(
                    expectedItem,
                    options => options.ComparingByMembers<Item>()
            );
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
