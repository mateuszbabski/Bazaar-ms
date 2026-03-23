namespace Bazaar.Baskets.Service.Commands.Handlers.AddProductToBasket
{
    public class AddProductToBasketCommand
    {
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
    }
}
