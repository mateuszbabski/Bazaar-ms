namespace Bazaar.Baskets.Service.Commands.Handlers.ChangeBasketCurrency
{
    public class ChangeBasketCurrencyCommand
    {
        public Guid CustomerId { get; set; }
        public string Currency { get; set; }
    }
}
