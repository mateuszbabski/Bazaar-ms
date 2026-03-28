using Bazaar.Shared.Models.Models;
using System.Text.Json.Serialization;

namespace Bazaar.Orders.Service.Domain.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public Guid ShopId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
        public decimal WeightInKg { get; set; }
        [JsonIgnore]
        public virtual Order Order { get; set; }

        public static OrderItem Create(
            Guid orderId,
            Guid productId,
            Guid shopId,
            string name,
            int quantity,
            decimal price,
            Currency currency,
            decimal weightInKg)
        {
            return new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                ProductId = productId,
                ShopId = shopId,
                Name = name,
                Quantity = quantity,
                Price = price,
                Currency = currency,
                WeightInKg = weightInKg
            };
        }
    }
}
