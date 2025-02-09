using Products.API.Models;

namespace Products.API.Repositories
{
    public interface IProductRepo
    {
        Task<ProductPaginated> GetAllProducts(QueryParameters queryParameters);
        Task<Product> GetProductById(Guid id);
        Task CreateProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(Guid id);
    }
}
