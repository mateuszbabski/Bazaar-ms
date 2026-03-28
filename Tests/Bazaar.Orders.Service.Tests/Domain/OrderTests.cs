using Bazaar.Orders.Service.Domain.Models;
using Bazaar.Shared.Models.Models;

namespace Bazaar.Orders.Service.Tests.Domain
{
    public class OrderTests
    {
        [Fact]
        public void Create_ShouldReturnOrder_WithPendingStatus()
        {
            var customerId = Guid.NewGuid();
            var now = DateTime.UtcNow;

            var order = Order.Create(customerId, 100m, Currency.PLN, 2.5m, now);

            Assert.NotEqual(Guid.Empty, order.Id);
            Assert.Equal(customerId, order.CustomerId);
            Assert.Equal(100m, order.TotalPrice);
            Assert.Equal(Currency.PLN, order.Currency);
            Assert.Equal(2.5m, order.TotalWeightInKg);
            Assert.Equal(OrderStatus.Pending, order.Status);
            Assert.Equal(now, order.CreatedAt);
            Assert.Empty(order.Items);
        }

        [Fact]
        public void Pay_ShouldChangeStatus_ToPaid()
        {
            var order = Order.Create(Guid.NewGuid(), 100m, Currency.PLN, 2.5m, DateTime.UtcNow);

            order.Pay();

            Assert.Equal(OrderStatus.Paid, order.Status);
        }

        [Fact]
        public void Cancel_ShouldChangeStatus_ToCancelled()
        {
            var order = Order.Create(Guid.NewGuid(), 100m, Currency.PLN, 2.5m, DateTime.UtcNow);

            order.Cancel();

            Assert.Equal(OrderStatus.Cancelled, order.Status);
        }

        [Fact]
        public void Complete_ShouldChangeStatus_ToCompleted()
        {
            var order = Order.Create(Guid.NewGuid(), 100m, Currency.PLN, 2.5m, DateTime.UtcNow);

            order.Complete();

            Assert.Equal(OrderStatus.Completed, order.Status);
        }
    }
}
