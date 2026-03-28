namespace Bazaar.Baskets.Service.Commands.Dtos
{
    public class CurrencyRateFetchResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public decimal Rate { get; set; }

        public static CurrencyRateFetchResult Success(decimal rate)
            => new() { IsSuccess = true, Rate = rate };

        public static CurrencyRateFetchResult ServiceUnavailable()
            => new() { IsSuccess = false, ErrorMessage = "Currency rate service is unavailable." };

        public static CurrencyRateFetchResult Error(string message)
            => new() { IsSuccess = false, ErrorMessage = message };
    }
}
