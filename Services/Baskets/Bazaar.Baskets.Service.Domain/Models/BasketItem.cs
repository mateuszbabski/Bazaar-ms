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

        public void ChangeBasketItemQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            var weightPerUnit = this.WeightInKg / this.Quantity;
            this.Quantity = quantity;
            this.Price = CalculateTotalPrice(this.BasePrice, quantity);
            this.WeightInKg = CalculateTotalWeight(weightPerUnit, quantity);
        }
    }
}
