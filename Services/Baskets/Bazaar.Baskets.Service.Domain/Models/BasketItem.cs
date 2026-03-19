using Bazaar.Shared.Models.Models;
using System.Text.Json.Serialization;

namespace Bazaar.Baskets.Service.Domain.Models
{
    public class BasketItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid ShopId { get; set; }
        public Guid BasketId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
        public decimal BasePrice { get; set; }
        public Currency BaseCurrency { get; set; }
        public decimal WeightInKg { get; set; }
        [JsonIgnore]
        public virtual Basket Basket { get; set; }

        public static BasketItem Create(
            Guid productId,
            Guid shopId,
            Guid basketId,
            string name,
            int quantity,
            decimal basePrice,
            Currency baseCurrency,
            decimal weightInKg)
        {
            return new BasketItem
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                ShopId = shopId,
                BasketId = basketId,
                Name = name,
                Quantity = quantity,
                Price = CalculateTotalPrice(basePrice, quantity),
                Currency = baseCurrency,
                BasePrice = basePrice,
                BaseCurrency = baseCurrency,
                WeightInKg = CalculateTotalWeight(weightInKg, quantity)
            };
        }

        public static decimal CalculateTotalPrice(decimal price, int quantity)
        {
            return price * quantity;
        }

        public static decimal CalculateTotalWeight(decimal weightPerUnit, int quantity)
        {
            return weightPerUnit * quantity;
        }

        public void ChangeCurrency(decimal conversionRate, Currency newCurrency)
        {
            if (this.Currency != newCurrency)
            {
                this.Currency = newCurrency;
                this.Price *= conversionRate;
            }
        }

        public void ChangeBasketItemQuantity(int quantity, decimal price, Currency currency)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            this.WeightInKg = CalculateTotalWeight(this.WeightInKg / this.Quantity, quantity);
            this.Quantity = quantity;
            this.Price = price * quantity;
        }
    }
}
