namespace Bazaar.Baskets.Service.Commands.Dtos
{
    public class ProductFetchResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public ProductDto? Product { get; set; }

        public static ProductFetchResult Success(ProductDto product)
            => new() { IsSuccess = true, Product = product };

        public static ProductFetchResult NotFound(Guid productId)
            => new() { IsSuccess = false, ErrorMessage = $"Product with ID {productId} was not found." };

        public static ProductFetchResult ServiceUnavailable()
            => new() { IsSuccess = false, ErrorMessage = "Products service is unavailable." };

        public static ProductFetchResult Error(string message)
            => new() { IsSuccess = false, ErrorMessage = message };
    }
}
