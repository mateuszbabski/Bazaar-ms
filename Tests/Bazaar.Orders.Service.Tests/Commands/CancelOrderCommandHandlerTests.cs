using Bazaar.Orders.Service.Commands.Handlers.CancelOrder;
using Bazaar.Orders.Service.DataAccess.Repository;
using Bazaar.Orders.Service.Domain.ErrorCodes;
using Bazaar.Orders.Service.Domain.Models;
using Bazaar.Shared.Models.Models;
using NSubstitute;

namespace Bazaar.Orders.Service.Tests.Commands
{
    public class CancelOrderCommandHandlerTests
    {
        private readonly IOrderRepository _orderRepository;
        private readonly CancelOrderCommandHandler _handler;

        public CancelOrderCommandHandlerTests()
        {
            _orderRepository = Substitute.For<IOrderRepository>();
            _handler = new CancelOrderCommandHandler(_orderRepository);
        }

        [Fact]
        public async Task Handle_WithValidPendingOrder_ShouldReturnSuccess()
        {
            var customerId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var order = Order.Create(customerId, 100m, Currency.PLN, 2.5m, DateTime.UtcNow);
            order.Id = orderId;

            _orderRepository.GetById(orderId).Returns(order);

            var command = new CancelOrderCommand { OrderId = orderId, CustomerId = customerId };

            var result = await _handler.Handle(command);

            Assert.True(result.IsSuccess);
            Assert.Equal(OrderStatus.Cancelled, order.Status);
            await _orderRepository.Received(1).Update(order);
        }

        [Fact]
        public async Task Handle_WithNonExistentOrder_ShouldReturnOrderNotFound()
        {
            var orderId = Guid.NewGuid();
            _orderRepository.GetById(orderId).Returns((Order)null!);

            var command = new CancelOrderCommand { OrderId = orderId, CustomerId = Guid.NewGuid() };

            var result = await _handler.Handle(command);

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

            var command = new CancelOrderCommand { OrderId = orderId, CustomerId = Guid.NewGuid() };

            var result = await _handler.Handle(command);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.Forbidden, result.ErrorCode);
        }

        [Theory]
        [InlineData(OrderStatus.Paid)]
        [InlineData(OrderStatus.Cancelled)]
        [InlineData(OrderStatus.Completed)]
        public async Task Handle_WithNonPendingOrder_ShouldReturnInvalidOrderStatus(OrderStatus status)
        {
            var customerId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var order = Order.Create(customerId, 100m, Currency.PLN, 2.5m, DateTime.UtcNow);
            order.Id = orderId;
            order.Status = status;

            _orderRepository.GetById(orderId).Returns(order);

            var command = new CancelOrderCommand { OrderId = orderId, CustomerId = customerId };

            var result = await _handler.Handle(command);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.InvalidOrderStatus, result.ErrorCode);
        }
    }
}
