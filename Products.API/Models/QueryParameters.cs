namespace Products.API.Models
{
    public class QueryParameters
    {
        public string? Search { get; set; } = "";
        public string? SortBy { get; set; } = "Name";
        public bool IsDescending { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
