using Bazaar.Orders.Service.Commands.Handlers.PlaceOrder;
using Bazaar.Orders.Service.DataAccess.Repository;
using Bazaar.Orders.Service.Domain.ErrorCodes;
using Bazaar.Orders.Service.Domain.Models;
using Bazaar.Shared.Infrastructure.TimeProvider;
using Bazaar.Shared.Models.Models;
using NSubstitute;

namespace Bazaar.Orders.Service.Tests.Commands
{
    public class PlaceOrderCommandHandlerTests
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly PlaceOrderCommandHandler _handler;

        public PlaceOrderCommandHandlerTests()
        {
            _orderRepository = Substitute.For<IOrderRepository>();
            _dateTimeProvider = Substitute.For<IDateTimeProvider>();
            _dateTimeProvider.UtcNow.Returns(new DateTime(2025, 1, 1, 12, 0, 0));
            _handler = new PlaceOrderCommandHandler(_orderRepository, _dateTimeProvider);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldReturnSuccess()
        {
            var command = new PlaceOrderCommand
            {
                CustomerId = Guid.NewGuid(),
                TotalPrice = 100m,
                Currency = Currency.PLN,
                TotalWeightInKg = 2.5m,
                Items =
                [
                    new PlaceOrderItemCommand
                    {
                        ProductId = Guid.NewGuid(),
                        ShopId = Guid.NewGuid(),
                        Name = "Test Product",
                        Quantity = 1,
                        Price = 100m,
                        Currency = Currency.PLN,
                        WeightInKg = 2.5m
                    }
                ]
            };

            var result = await _handler.Handle(command);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.OrderId);
            await _orderRepository.Received(1).Add(Arg.Any<Order>());
        }

        [Fact]
        public async Task Handle_WithEmptyItems_ShouldReturnValidationFailed()
        {
            var command = new PlaceOrderCommand
            {
                CustomerId = Guid.NewGuid(),
                TotalPrice = 100m,
                Currency = Currency.PLN,
                TotalWeightInKg = 2.5m,
                Items = []
            };

            var result = await _handler.Handle(command);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.ValidationFailed, result.ErrorCode);
            await _orderRepository.DidNotReceive().Add(Arg.Any<Order>());
        }

        [Fact]
        public async Task Handle_WithMultipleItems_ShouldCreateOrderWithAllItems()
        {
            var command = new PlaceOrderCommand
            {
                CustomerId = Guid.NewGuid(),
                TotalPrice = 200m,
                Currency = Currency.PLN,
                TotalWeightInKg = 5m,
                Items =
                [
                    new PlaceOrderItemCommand
                    {
                        ProductId = Guid.NewGuid(),
                        ShopId = Guid.NewGuid(),
                        Name = "Product 1",
                        Quantity = 1,
                        Price = 100m,
                        Currency = Currency.PLN,
                        WeightInKg = 2.5m
                    },
                    new PlaceOrderItemCommand
                    {
                        ProductId = Guid.NewGuid(),
                        ShopId = Guid.NewGuid(),
                        Name = "Product 2",
                        Quantity = 2,
                        Price = 100m,
                        Currency = Currency.PLN,
                        WeightInKg = 2.5m
                    }
                ]
            };

            var result = await _handler.Handle(command);

            Assert.True(result.IsSuccess);
            await _orderRepository.Received(1).Add(Arg.Is<Order>(o => o.Items.Count == 2));
        }
    }
}
