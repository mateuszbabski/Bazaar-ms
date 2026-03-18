namespace Bazaar.Products.Service.Queries.Handlers.SearchProducts
{
    public class SearchProductsQuery
    {
        public string? Name { get; set; }
        public string? Category { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
