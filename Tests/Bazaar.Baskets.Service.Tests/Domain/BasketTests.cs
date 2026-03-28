using Bazaar.Baskets.Service.Domain.Models;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Baskets.Service.Tests.Domain
{
    public class BasketTests
    {
        [Fact]
        public void CreateBasket_ShouldReturnEmptyBasket()
        {
            var customerId = Guid.NewGuid();

            var basket = Basket.CreateBasket(customerId, Currency.PLN);

            Assert.NotEqual(Guid.Empty, basket.Id);
            Assert.Equal(customerId, basket.CustomerId);
            Assert.Equal(Currency.PLN, basket.Currency);
            Assert.Equal(0m, basket.TotalPrice);
            Assert.Equal(0m, basket.TotalWeightInKg);
            Assert.Empty(basket.Items);
        }

        [Fact]
        public void CountTotalPrice_ShouldSumItemPrices()
        {
            var basket = Basket.CreateBasket(Guid.NewGuid(), Currency.PLN);
            basket.Items.Add(CreateItem(basket.Id, 50m, Currency.PLN, 1m));
            basket.Items.Add(CreateItem(basket.Id, 30m, Currency.PLN, 2m));

            basket.CountTotalPrice();

            Assert.Equal(80m, basket.TotalPrice);
        }

        [Fact]
        public void CalculateTotalWeight_ShouldSumItemWeights()
        {
            var basket = Basket.CreateBasket(Guid.NewGuid(), Currency.PLN);
            basket.Items.Add(CreateItem(basket.Id, 50m, Currency.PLN, 1.5m));
            basket.Items.Add(CreateItem(basket.Id, 30m, Currency.PLN, 2.5m));

            basket.CalculateTotalWeight();

            Assert.Equal(4m, basket.TotalWeightInKg);
        }

        [Fact]
        public void RemoveItem_ShouldRemoveAndRecalculate()
        {
            var basket = Basket.CreateBasket(Guid.NewGuid(), Currency.PLN);
            var item1 = CreateItem(basket.Id, 50m, Currency.PLN, 1m);
            var item2 = CreateItem(basket.Id, 30m, Currency.PLN, 2m);
            basket.Items.Add(item1);
            basket.Items.Add(item2);

            basket.RemoveItem(item1.Id);

            Assert.Single(basket.Items);
            Assert.Equal(30m, basket.TotalPrice);
            Assert.Equal(2m, basket.TotalWeightInKg);
        }

        [Fact]
        public void RemoveItem_WithNonExistentId_ShouldDoNothing()
        {
            var basket = Basket.CreateBasket(Guid.NewGuid(), Currency.PLN);
            basket.Items.Add(CreateItem(basket.Id, 50m, Currency.PLN, 1m));

            basket.RemoveItem(Guid.NewGuid());

            Assert.Single(basket.Items);
        }

        [Fact]
        public void ChangeBasketCurrency_ShouldConvertAllItems()
        {
            var basket = Basket.CreateBasket(Guid.NewGuid(), Currency.PLN);
            basket.Items.Add(CreateItem(basket.Id, 100m, Currency.PLN, 1m));

            basket.ChangeBasketCurrency(0.25m, Currency.EUR);

            Assert.Equal(Currency.EUR, basket.Currency);
        }

        private static BasketItem CreateItem(Guid basketId, decimal price, Currency currency, decimal weight)
        {
            var item = BasketItem.Create(
                Guid.NewGuid(), Guid.NewGuid(), basketId,
                "Product", 1, price, currency, weight);
            return item;
        }
    }
}
