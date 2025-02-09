namespace Products.API.Models
{
    public class ProductPaginated
    {
        public required IList<Product> Products { get; set; }
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
