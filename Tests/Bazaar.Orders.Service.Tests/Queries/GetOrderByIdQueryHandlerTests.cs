using Bazaar.Orders.Service.DataAccess.Repository;
using Bazaar.Orders.Service.Domain.ErrorCodes;
using Bazaar.Orders.Service.Domain.Models;
using Bazaar.Orders.Service.Queries.Handlers.GetOrderById;
using Bazaar.Shared.Models.Models;
using NSubstitute;

namespace Bazaar.Orders.Service.Tests.Queries
{
    public class GetOrderByIdQueryHandlerTests
    {
        private readonly IOrderRepository _orderRepository;
        private readonly GetOrderByIdQueryHandler _handler;

        public GetOrderByIdQueryHandlerTests()
        {
            _orderRepository = Substitute.For<IOrderRepository>();
            _handler = new GetOrderByIdQueryHandler(_orderRepository);
        }

        [Fact]
        public async Task Handle_WithExistingOrder_ShouldReturnSuccess()
        {
            var customerId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var order = Order.Create(customerId, 100m, Currency.PLN, 2.5m, DateTime.UtcNow);
            order.Id = orderId;
            order.Items.Add(OrderItem.Create(orderId, Guid.NewGuid(), Guid.NewGuid(), "Product", 1, 100m, Currency.PLN, 2.5m));

            _orderRepository.GetById(orderId).Returns(order);

            var query = new GetOrderByIdQuery { OrderId = orderId, CustomerId = customerId };

            var result = await _handler.Handle(query);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Order);
            Assert.Equal(orderId, result.Order.Id);
            Assert.Single(result.Order.Items);
        }

        [Fact]
        public async Task Handle_WithNonExistentOrder_ShouldReturnOrderNotFound()
        {
            var orderId = Guid.NewGuid();
            _orderRepository.GetById(orderId).Returns((Order)null!);

            var query = new GetOrderByIdQuery { OrderId = orderId, CustomerId = Guid.NewGuid() };

            var result = await _handler.Handle(query);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.OrderNotFound, result.ErrorCode);
        }

        [Fact]
        public async Task Handle_WithDifferentCustomer_ShouldReturnForbidden()
        {
            var orderId = Guid.NewGuid();
            var order = Order.Create(Guid.NewGuid(), 100m, Currency.PLN, 2.5m, DateTime.UtcNow);
            order.Id = orderId;

            _orderRepository.GetById(orderId).Returns(order);

            var query = new GetOrderByIdQuery { OrderId = orderId, CustomerId = Guid.NewGuid() };

            var result = await _handler.Handle(query);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.Forbidden, result.ErrorCode);
        }
    }
}
