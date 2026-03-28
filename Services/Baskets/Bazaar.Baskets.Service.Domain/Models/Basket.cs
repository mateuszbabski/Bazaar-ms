using Bazaar.Shared.Models.Models;

namespace Bazaar.Baskets.Service.Domain.Models
{
    public class Basket
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public Currency Currency { get; set; }
        public decimal TotalWeightInKg { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

        public static Basket CreateBasket(Guid customerId, Currency currency)
        {
            return new Basket
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                Currency = currency,
                TotalPrice = 0,
                TotalWeightInKg = 0,
                Items = new List<BasketItem>()
            };
        }

        public void CountTotalPrice()
        {
            TotalPrice = Items.Sum(item => item.Price);
        }

        public void CalculateTotalWeight()
        {
            TotalWeightInKg = Items.Sum(item => item.WeightInKg);
        }

        public void ChangeBasketCurrency(decimal conversionRate, Currency newCurrency)
        {
            foreach (var item in Items) 
            {
                item.ChangeCurrency(conversionRate, newCurrency);
            }

            Currency = newCurrency;
            TotalPrice = Items.Sum(x => x.Price);
        }

        public void RemoveItem(Guid basketItemId)
        {
            var item = Items.FirstOrDefault(i => i.Id == basketItemId);
            if (item != null)
            {
                Items.Remove(item);
                CountTotalPrice();
                CalculateTotalWeight();
            }
        }
    }
}
