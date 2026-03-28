namespace Bazaar.Baskets.Service.Commands.Dtos
{
    public class CurrencyRateResponse
    {
        public string? Base { get; set; }
        public Dictionary<string, decimal>? Rates { get; set; }
    }
}
