namespace Bazaar.Baskets.Service.Commands.Dtos
{
    public class ProductServiceResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }
        public ProductDto? Product { get; set; }
    }
}
