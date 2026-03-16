using Bazaar.Shared.Models.Models;

namespace Bazaar.Products.Service.Domain.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
        public string Unit { get; set; }
        public decimal WeightPerUnitInKg { get; set; } 
        public bool IsAvailable { get; set; }

        public static Product Create(
            Guid customerId, 
            string name, 
            string description, 
            string category, 
            decimal price, 
            Currency currency, 
            string unit, 
            decimal weightPerUnit, 
            bool isAvailable = true)
        {
            return new Product
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                Name = name,
                Description = description,
                Category = category,
                Price = price,
                Currency = currency,
                Unit = unit,
                WeightPerUnitInKg = weightPerUnit,
                IsAvailable = isAvailable
            };
        }

        public void ChangeProductDetails(
            string name, 
            string description, 
            string unit, 
            decimal weightPerUnit)
        {
            if (!string.IsNullOrEmpty(name))
                Name = name;
            if (!string.IsNullOrEmpty(description))
                Description = description;
            if (!string.IsNullOrEmpty(unit))
                Unit = unit;
            if (weightPerUnit > 0)
                WeightPerUnitInKg = weightPerUnit;
        }

        public void ChangeProductPrice(decimal price, Currency currency)
        {
            if (price > 0)
                Price = price;
            if (Enum.IsDefined(currency))
                Currency = currency;
        }

        public void ChangeProductAvailability(bool isAvailable)
        {
            IsAvailable = isAvailable;
        }
    }
}
