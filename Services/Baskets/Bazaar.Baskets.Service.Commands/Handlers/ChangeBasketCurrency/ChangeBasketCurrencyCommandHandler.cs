using Bazaar.Baskets.Service.Commands.Clients;
using Bazaar.Baskets.Service.Commands.Responses;
using Bazaar.Baskets.Service.DataAccess.Repository;
using Bazaar.Baskets.Service.Domain.ErrorCodes;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Baskets.Service.Commands.Handlers.ChangeBasketCurrency
{
    public class ChangeBasketCurrencyCommandHandler
    {
        private readonly IBasketRepository _basketRepository;
        private readonly CurrencyRateClient _currencyRateClient;

        public ChangeBasketCurrencyCommandHandler(
            IBasketRepository basketRepository,
            CurrencyRateClient currencyRateClient)
        {
            _basketRepository = basketRepository;
            _currencyRateClient = currencyRateClient;
        }

        public async Task<ChangeBasketCurrencyResult> Handle(ChangeBasketCurrencyCommand command)
        {
            if (!Enum.TryParse<Currency>(command.Currency, ignoreCase: true, out var newCurrency))
                return ChangeBasketCurrencyResult.Failure($"Invalid currency: {command.Currency}.", ErrorCode.ValidationFailed);

            var basket = await _basketRepository.GetByCustomerId(command.CustomerId);
            if (basket == null)
                return ChangeBasketCurrencyResult.Failure("Basket not found.", ErrorCode.BasketNotFound);

            if (basket.Currency == newCurrency)
                return ChangeBasketCurrencyResult.Success("Basket already uses this currency.");

            var rateResult = await _currencyRateClient.GetConversionRate(basket.Currency, newCurrency);
            if (!rateResult.IsSuccess)
                return ChangeBasketCurrencyResult.Failure(rateResult.ErrorMessage!, ErrorCode.CurrencyConversionFailed);

            basket.ChangeBasketCurrency(rateResult.Rate, newCurrency);

            try
            {
                await _basketRepository.Update(basket);
                return ChangeBasketCurrencyResult.Success($"Basket currency changed to {newCurrency}.");
            }
            catch
            {
                return ChangeBasketCurrencyResult.Failure("Failed to update basket.", ErrorCode.UnknownError);
            }
        }
    }
}
