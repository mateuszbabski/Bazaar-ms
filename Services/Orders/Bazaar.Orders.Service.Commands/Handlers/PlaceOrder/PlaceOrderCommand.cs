using Bazaar.Shared.Models.Models;

namespace Bazaar.Orders.Service.Commands.Handlers.PlaceOrder
{
    public class PlaceOrderCommand
    {
        public Guid CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public Currency Currency { get; set; }
        public decimal TotalWeightInKg { get; set; }
        public List<PlaceOrderItemCommand> Items { get; set; } = [];
    }

    public class PlaceOrderItemCommand
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
