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

        public AddProductToBasketCommandHandler(
            IBasketRepository basketRepository,
            ProductServiceClient productServiceClient)
        {
            _basketRepository = basketRepository;
            _productServiceClient = productServiceClient;
        }

        public async Task<AddProductToBasketResult> Handle(AddProductToBasketCommand command)
        {            
            var productResult = await _productServiceClient.GetProductById(command.ProductId);
            if (!productResult.IsSuccess)
            {
                return AddProductToBasketResult.Failure(productResult.ErrorMessage!, ErrorCode.ProductNotFound);
            }

            var product = productResult.Product!;

            var basket = await _basketRepository.GetByCustomerId(command.CustomerId);
            if (basket == null)
            {
                basket = Basket.CreateBasket(command.CustomerId, product.Currency);                
                await _basketRepository.Add(basket);
            }

            AddOrUpdateBasketItem(basket, product, command.Quantity);
            basket.CountTotalPrice();
            basket.CalculateTotalWeight();

            try
            {
                await _basketRepository.Update(basket);
                return AddProductToBasketResult.Success("Product added to basket successfully");
            }
            catch
            {
                return AddProductToBasketResult.Failure("Failed to update basket", ErrorCode.UnknownError);
            }
        }

        private static void AddOrUpdateBasketItem(Basket basket, ProductDto product, int quantity)
        {
            var existingItem = basket.Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.Price = BasketItem.CalculateTotalPrice(existingItem.BasePrice, existingItem.Quantity);
                existingItem.WeightInKg = BasketItem.CalculateTotalWeight(product.WeightPerUnitInKg, existingItem.Quantity);
            }
            else
            {
                var newItem = BasketItem.Create(
                    product.Id,
                    product.CustomerId,
                    basket.Id,
                    product.Name,
                    quantity,
                    product.Price,
                    product.Currency,
                    product.WeightPerUnitInKg);
                basket.Items.Add(newItem);
            }
        }
    }
}
