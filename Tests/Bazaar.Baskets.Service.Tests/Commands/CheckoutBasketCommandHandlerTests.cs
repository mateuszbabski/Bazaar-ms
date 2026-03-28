using Bazaar.Baskets.Service.Commands.Clients;
using Bazaar.Baskets.Service.Commands.Handlers.CheckoutBasket;
using Bazaar.Baskets.Service.DataAccess.Repository;
using Bazaar.Baskets.Service.Domain.ErrorCodes;
using Bazaar.Baskets.Service.Domain.Models;
using Bazaar.Shared.Models.Models;
using NSubstitute;

namespace Bazaar.Baskets.Service.Tests.Commands
{
    public class CheckoutBasketCommandHandlerTests
    {
        private readonly IBasketRepository _basketRepository;
        private readonly OrderServiceClient _orderServiceClient;
        private readonly CheckoutBasketCommandHandler _handler;

        public CheckoutBasketCommandHandlerTests()
        {
            _basketRepository = Substitute.For<IBasketRepository>();
            _orderServiceClient = Substitute.For<OrderServiceClient>();
            _handler = new CheckoutBasketCommandHandler(_basketRepository, _orderServiceClient);
        }

        [Fact]
        public async Task Handle_WithNonExistentBasket_ShouldReturnBasketNotFound()
        {
            var customerId = Guid.NewGuid();
            _basketRepository.GetByCustomerId(customerId).Returns((Basket)null!);

            var command = new CheckoutBasketCommand { CustomerId = customerId };

            var result = await _handler.Handle(command);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.BasketNotFound, result.ErrorCode);
        }

        [Fact]
        public async Task Handle_WithEmptyBasket_ShouldReturnBasketEmpty()
        {
            var customerId = Guid.NewGuid();
            var basket = Basket.CreateBasket(customerId, Currency.PLN);

            _basketRepository.GetByCustomerId(customerId).Returns(basket);

            var command = new CheckoutBasketCommand { CustomerId = customerId };

            var result = await _handler.Handle(command);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.BasketEmpty, result.ErrorCode);
        }
    }
}
