namespace Bazaar.Api.Gateway.Requests.Baskets
{
    public class AddProductToBasketRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
