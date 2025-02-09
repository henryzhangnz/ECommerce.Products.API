using Microsoft.EntityFrameworkCore;
using Products.API.Data;
using Products.API.Models;

namespace Products.API.Repositories
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext _dbContext;
        private static readonly string[] AllowedSortColumns = { "Name", "Description", "Price", "Amount" };

        public ProductRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            }

            if (await _dbContext.Products.AnyAsync(p => p.Id == product.Id))
            {
                throw new InvalidOperationException("Product with the same ID already exists.");
            }

            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Product> GetProductById(Guid id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            return product ?? throw new KeyNotFoundException($"Product with ID {id} not found.");
        }

        public async Task<ProductPaginated> GetAllProducts(QueryParameters queryParams)
        {
            if (queryParams.Page < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(queryParams.Page), $"Invalid page number {queryParams.Page}");
            }

            if (queryParams.Search is null)
            {
                queryParams.Search = "";
            }

            if (queryParams.Search.Length > 100)
            {
                throw new ArgumentException("Search query is too long.", nameof(queryParams.Search));
            }

            var query = _dbContext.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                query = query.Where(p =>
                    p.Name!.Contains(queryParams.Search) ||
                    p.Description!.Contains(queryParams.Search));
            }

            if (string.IsNullOrWhiteSpace(queryParams.SortBy) || !AllowedSortColumns.Contains(queryParams.SortBy))
            {
                queryParams.SortBy = "Name";
            }

            query = queryParams.IsDescending
                ? query.OrderByDescending(p => EF.Property<object>(p, queryParams.SortBy!))
                : query.OrderBy(p => EF.Property<object>(p, queryParams.SortBy!));

            var totalItems = await query.CountAsync();
            var products = await query
                .Skip((queryParams.Page - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            return new ProductPaginated
            {
                Products = products,
                TotalItems = totalItems,
                Page = queryParams.Page,
                PageSize = queryParams.PageSize
            };
        }

        public async Task UpdateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            }

            var existingProduct = await _dbContext.Products.FindAsync(product.Id) ?? 
                                throw new KeyNotFoundException($"Product with ID {product.Id} not found.");
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProduct(Guid id)
        {
            var product = await _dbContext.Products.FindAsync(id) ?? 
                        throw new KeyNotFoundException($"Product with ID {id} not found.");
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}
