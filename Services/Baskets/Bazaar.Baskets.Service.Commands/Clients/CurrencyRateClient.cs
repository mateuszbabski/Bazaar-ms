using Bazaar.Baskets.Service.Commands.Dtos;
using Bazaar.Shared.Models.Models;
using System.Net.Http.Json;

namespace Bazaar.Baskets.Service.Commands.Clients
{
    public class CurrencyRateClient
    {
        private readonly HttpClient _httpClient;

        public CurrencyRateClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://open.er-api.com/v6/latest/");
        }

        public async Task<CurrencyRateFetchResult> GetConversionRate(Currency from, Currency to)
        {
            if (from == to)
                return CurrencyRateFetchResult.Success(1m);

            try
            {
                var response = await _httpClient.GetAsync(from.ToString());

                if (!response.IsSuccessStatusCode)
                    return CurrencyRateFetchResult.Error($"Currency rate API returned {(int)response.StatusCode}.");

                var result = await response.Content.ReadFromJsonAsync<CurrencyRateResponse>();
                if (result?.Rates is null)
                    return CurrencyRateFetchResult.Error("Failed to deserialize currency rate response.");

                if (!result.Rates.TryGetValue(to.ToString(), out var rate))
                    return CurrencyRateFetchResult.Error($"Rate for {to} not found.");

                return CurrencyRateFetchResult.Success(rate);
            }
            catch (HttpRequestException)
            {
                return CurrencyRateFetchResult.ServiceUnavailable();
            }
            catch (TaskCanceledException)
            {
                return CurrencyRateFetchResult.ServiceUnavailable();
            }
        }
    }
}
