using Bazaar.Orders.Service.DataAccess.Repository;
using Bazaar.Orders.Service.Domain.ErrorCodes;
using Bazaar.Orders.Service.Domain.Models;
using Bazaar.Orders.Service.Queries.Handlers.GetOrdersByShopId;
using Bazaar.Shared.Models.Models;
using NSubstitute;

namespace Bazaar.Orders.Service.Tests.Queries
{
    public class GetOrdersByShopIdQueryHandlerTests
    {
        private readonly IOrderRepository _orderRepository;
        private readonly GetOrdersByShopIdQueryHandler _handler;

        public GetOrdersByShopIdQueryHandlerTests()
        {
            _orderRepository = Substitute.For<IOrderRepository>();
            _handler = new GetOrdersByShopIdQueryHandler(_orderRepository);
        }

        [Fact]
        public async Task Handle_WithExistingOrders_ShouldReturnSuccess()
        {
            var shopId = Guid.NewGuid();
            var orders = new List<Order>
            {
                CreateOrder(shopId),
                CreateOrder(shopId)
            };

            _orderRepository.GetByShopId(shopId, null).Returns(orders);

            var query = new GetOrdersByShopIdQuery { ShopId = shopId };

            var result = await _handler.Handle(query);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Orders);
            Assert.Equal(2, result.Orders.Count);
        }

        [Fact]
        public async Task Handle_WithNoOrders_ShouldReturnOrdersNotFound()
        {
            var shopId = Guid.NewGuid();
            _orderRepository.GetByShopId(shopId, null).Returns(Enumerable.Empty<Order>());

            var query = new GetOrdersByShopIdQuery { ShopId = shopId };

            var result = await _handler.Handle(query);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.OrdersNotFound, result.ErrorCode);
        }

        private static Order CreateOrder(Guid shopId)
        {
            var order = Order.Create(Guid.NewGuid(), 100m, Currency.PLN, 2.5m, DateTime.UtcNow);
            order.Items.Add(OrderItem.Create(order.Id, Guid.NewGuid(), shopId, "Product", 1, 100m, Currency.PLN, 2.5m));
            return order;
        }
    }
}
