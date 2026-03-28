using Bazaar.Orders.Service.Domain.Models;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Orders.Service.Queries.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public Currency Currency { get; set; }
        public decimal TotalWeightInKg { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemDto> Items { get; set; } = [];
    }

    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid ShopId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
        public decimal WeightInKg { get; set; }
    }
}
