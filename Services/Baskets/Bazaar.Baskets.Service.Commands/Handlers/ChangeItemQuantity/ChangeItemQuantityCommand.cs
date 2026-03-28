namespace Bazaar.Baskets.Service.Commands.Handlers.ChangeItemQuantity
{
    public class ChangeItemQuantityCommand
    {
        public Guid CustomerId { get; set; }
        public Guid BasketItemId { get; set; }
        public int Quantity { get; set; }
    }
}
