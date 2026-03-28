using Bazaar.Orders.Service.DataAccess.Repository;
using Bazaar.Orders.Service.Domain.ErrorCodes;
using Bazaar.Orders.Service.Domain.Models;
using Bazaar.Orders.Service.Queries.Handlers.GetOrdersByCustomerId;
using Bazaar.Shared.Models.Models;
using NSubstitute;

namespace Bazaar.Orders.Service.Tests.Queries
{
    public class GetOrdersByCustomerIdQueryHandlerTests
    {
        private readonly IOrderRepository _orderRepository;
        private readonly GetOrdersByCustomerIdQueryHandler _handler;

        public GetOrdersByCustomerIdQueryHandlerTests()
        {
            _orderRepository = Substitute.For<IOrderRepository>();
            _handler = new GetOrdersByCustomerIdQueryHandler(_orderRepository);
        }

        [Fact]
        public async Task Handle_WithExistingOrders_ShouldReturnSuccess()
        {
            var customerId = Guid.NewGuid();
            var orders = new List<Order>
            {
                CreateOrder(customerId),
                CreateOrder(customerId)
            };

            _orderRepository.GetByCustomerId(customerId, null).Returns(orders);

            var query = new GetOrdersByCustomerIdQuery { CustomerId = customerId };

            var result = await _handler.Handle(query);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Orders);
            Assert.Equal(2, result.Orders.Count);
        }

        [Fact]
        public async Task Handle_WithNoOrders_ShouldReturnOrdersNotFound()
        {
            var customerId = Guid.NewGuid();
            _orderRepository.GetByCustomerId(customerId, null).Returns(Enumerable.Empty<Order>());

            var query = new GetOrdersByCustomerIdQuery { CustomerId = customerId };

            var result = await _handler.Handle(query);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.OrdersNotFound, result.ErrorCode);
        }

        [Fact]
        public async Task Handle_WithStatusFilter_ShouldPassStatusToRepository()
        {
            var customerId = Guid.NewGuid();
            var orders = new List<Order> { CreateOrder(customerId) };

            _orderRepository.GetByCustomerId(customerId, OrderStatus.Paid).Returns(orders);

            var query = new GetOrdersByCustomerIdQuery { CustomerId = customerId, Status = OrderStatus.Paid };

            var result = await _handler.Handle(query);

            Assert.True(result.IsSuccess);
            await _orderRepository.Received(1).GetByCustomerId(customerId, OrderStatus.Paid);
        }

        private static Order CreateOrder(Guid customerId)
        {
            var order = Order.Create(customerId, 100m, Currency.PLN, 2.5m, DateTime.UtcNow);
            order.Items.Add(OrderItem.Create(order.Id, Guid.NewGuid(), Guid.NewGuid(), "Product", 1, 100m, Currency.PLN, 2.5m));
            return order;
        }
    }
}
