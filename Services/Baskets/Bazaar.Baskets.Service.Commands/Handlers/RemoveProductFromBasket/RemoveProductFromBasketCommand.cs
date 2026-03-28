namespace Bazaar.Baskets.Service.Commands.Handlers.RemoveProductFromBasket
{
    public class RemoveProductFromBasketCommand
    {
        public Guid CustomerId { get; set; }
        public Guid BasketItemId { get; set; }
    }
}
