using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Products.API.Controllers;
using Products.API.MessageBus;
using Products.API.Models;
using Products.API.Repositories;

namespace Products.API.Tests.ControllerTests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductRepo> _productRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IMessagePublisher> _messagePublisherMock;
        private readonly ProductsController _productsController;

        public ProductsControllerTests()
        {
            _productRepoMock = new Mock<IProductRepo>();
            _mapperMock = new Mock<IMapper>();
            _messagePublisherMock = new Mock<IMessagePublisher>();
            _productsController = new ProductsController(_productRepoMock.Object,
                                                        _mapperMock.Object,
                                                        _messagePublisherMock.Object);
        }

        [Fact]
        public async void GetProducts_Should_ReturnAllProducts_WhenProductsAreFound()
        {
            // Arrange
            var queryParameters = new QueryParameters();
            var products = new List<Product>()
            {
                new() {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    Price = 100,
                    Description = "Test",
                    Amount = 1,
                    CreatedAt = DateTime.UtcNow,
                },
                new() {
                    Id = Guid.NewGuid(),
                    Name = "Test2",
                    Price = 100,
                    Description = "Test2",
                    Amount = 2,
                    CreatedAt = DateTime.UtcNow,
                },
            };

            var productPaginated = new ProductPaginated()
            {
                Products = products,
                Page = 1,
                TotalItems = 2,
                PageSize = 10
            };
            _productRepoMock.Setup(x => x.GetAllProducts(queryParameters)).ReturnsAsync(productPaginated);

            // Act
            var result = await _productsController.GetProducts(queryParameters);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(productPaginated, okResult.Value);
        }
    }
}
