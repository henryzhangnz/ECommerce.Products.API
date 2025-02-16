using Microsoft.EntityFrameworkCore;
using Products.API.Models;
using Products.API.Repositories;

namespace Products.API.Tests.RepositoryTests
{
    public class ProductRepoTests : IClassFixture<AppDbContextFixture>
    {
        private readonly AppDbContextFixture _dbContextFixture;
        private readonly ProductRepo _productRepo;

        public ProductRepoTests()
        {
            _dbContextFixture = new AppDbContextFixture();
            _productRepo = new ProductRepo(_dbContextFixture.AppDbContext);
        }

        [Fact]
        public async void CreateProduct_Should_ThrowArgumentNullException_WhenProductIsNull()
        {
            // Arrange
            // Act
            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _productRepo.CreateProduct(product: null));
        }

        [Fact]
        public async void CreateProduct_Should_ThrowInvalidOperationException_WhenProductExists()
        {
            // Arrange
            var product = new Product()
            {
                CreatedAt = DateTime.UtcNow,
                Description = "Test",
                Id = Guid.NewGuid(),
                Name = "Test",
                Amount = 1,
                Price = 100
            };

            await _dbContextFixture.AppDbContext.Products.AddAsync(product);
            await _dbContextFixture.AppDbContext.SaveChangesAsync();


            // Act
            var createProduct = async () => await _productRepo.CreateProduct(product);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => createProduct());
        }

        [Fact]
        public async void CreateProduct_Should_ReturnProduct_WhenAddingNewOne()
        {
            // Arrange
            var product = new Product()
            {
                CreatedAt = DateTime.UtcNow,
                Description = "Test",
                Id = Guid.NewGuid(),
                Amount = 1,
                Name = "Test",
                Price = 100
            };

            // Act
            await _productRepo.CreateProduct(product);

            // Assert
            Assert.Single(_dbContextFixture.AppDbContext.Products);
            Assert.Equal(await _dbContextFixture.AppDbContext.Products.FirstAsync(), product);
        }
    }
}
