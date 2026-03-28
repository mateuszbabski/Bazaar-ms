using Bazaar.Shared.Models.Models;

namespace Bazaar.Orders.Service.Domain.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public Currency Currency { get; set; }
        public decimal TotalWeightInKg { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public static Order Create(
            Guid customerId,
            decimal totalPrice,
            Currency currency,
            decimal totalWeightInKg,
            DateTime createdAt)
        {
            return new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                TotalPrice = totalPrice,
                Currency = currency,
                TotalWeightInKg = totalWeightInKg,
                Status = OrderStatus.Pending,
                CreatedAt = createdAt,
                Items = new List<OrderItem>()
            };
        }

        public void Pay()
        {
            Status = OrderStatus.Paid;
        }

        public void Complete()
        {
            Status = OrderStatus.Completed;
        }

        public void Cancel()
        {
            Status = OrderStatus.Cancelled;
        }
    }
}
