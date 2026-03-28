using Bazaar.Baskets.Service.Commands.Clients;
using Bazaar.Baskets.Service.Commands.Dtos;
using Bazaar.Baskets.Service.Commands.Responses;
using Bazaar.Baskets.Service.DataAccess.Repository;
using Bazaar.Baskets.Service.Domain.ErrorCodes;
using Bazaar.Baskets.Service.Domain.Models;

namespace Bazaar.Baskets.Service.Commands.Handlers.AddProductToBasket
{
    public class AddProductToBasketCommandHandler
    {
        private readonly IBasketRepository _basketRepository;
        private readonly ProductServiceClient _productServiceClient;
        private readonly CurrencyRateClient _currencyRateClient;

        public AddProductToBasketCommandHandler(
            IBasketRepository basketRepository,
            ProductServiceClient productServiceClient,
            CurrencyRateClient currencyRateClient)
        {
            _basketRepository = basketRepository;
            _productServiceClient = productServiceClient;
            _currencyRateClient = currencyRateClient;
        }

        public async Task<AddProductToBasketResult> Handle(AddProductToBasketCommand command)
        {
            if (command.Quantity <= 0)
                return AddProductToBasketResult.Failure("Quantity must be greater than zero.", ErrorCode.ValidationFailed);

            var productResult = await _productServiceClient.GetProductById(command.ProductId);
            if (!productResult.IsSuccess)
            {
                return AddProductToBasketResult.Failure(productResult.ErrorMessage!, ErrorCode.ProductNotFound);
            }

            var product = productResult.Product!;

            if (!product.IsAvailable)
                return AddProductToBasketResult.Failure("Product is not available.", ErrorCode.ValidationFailed);

            var basket = await _basketRepository.GetByCustomerId(command.CustomerId);
            var isNewBasket = basket == null;

            if (isNewBasket)
                basket = Basket.CreateBasket(command.CustomerId, product.Currency);

            var convertedPrice = product.Price;
            if (product.Currency != basket.Currency)
            {
                var rateResult = await _currencyRateClient.GetConversionRate(product.Currency, basket.Currency);
                if (!rateResult.IsSuccess)
                    return AddProductToBasketResult.Failure(rateResult.ErrorMessage!, ErrorCode.CurrencyConversionFailed);

                convertedPrice = product.Price * rateResult.Rate;
            }

            AddOrUpdateBasketItem(basket, product, command.Quantity, convertedPrice, basket.Currency);
            basket.CountTotalPrice();
            basket.CalculateTotalWeight();

            try
            {
                if (isNewBasket)
                    await _basketRepository.Add(basket);
                else
                    await _basketRepository.Update(basket);

                return AddProductToBasketResult.Success("Product added to basket successfully");
            }
            catch (Exception ex)
            {
                return AddProductToBasketResult.Failure($"Failed to update basket: {ex.Message}", ErrorCode.UnknownError);
            }
        }

        private static void AddOrUpdateBasketItem(
            Basket basket, ProductDto product, int quantity, decimal convertedPrice, Shared.Models.Models.Currency basketCurrency)
        {
            var existingItem = basket.Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.Price = BasketItem.CalculateTotalPrice(convertedPrice, existingItem.Quantity);
                existingItem.WeightInKg = BasketItem.CalculateTotalWeight(product.WeightPerUnitInKg, existingItem.Quantity);
            }
            else
            {
                var newItem = BasketItem.Create(
                    product.Id,
                    product.ShopId,
                    basket.Id,
                    product.Name,
                    quantity,
                    convertedPrice,
                    basketCurrency,
                    product.WeightPerUnitInKg);
                basket.Items.Add(newItem);
            }
        }
    }
}
