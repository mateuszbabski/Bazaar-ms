using Bazaar.Shared.Models.Models;

namespace Bazaar.Baskets.Service.Commands.Dtos
{
    public class CreateOrderRequest
    {
        public Guid CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public Currency Currency { get; set; }
        public decimal TotalWeightInKg { get; set; }
        public List<CreateOrderItemRequest> Items { get; set; } = [];
    }

    public class CreateOrderItemRequest
    {
        public Guid ProductId { get; set; }
        public Guid ShopId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
        public decimal WeightInKg { get; set; }
    }
}
