using Bazaar.Baskets.Service.Commands.Handlers.ClearBasket;
using Bazaar.Baskets.Service.DataAccess.Repository;
using Bazaar.Baskets.Service.Domain.ErrorCodes;
using Bazaar.Baskets.Service.Domain.Models;
using Bazaar.Shared.Models.Models;
using NSubstitute;

namespace Bazaar.Baskets.Service.Tests.Commands
{
    public class ClearBasketCommandHandlerTests
    {
        private readonly IBasketRepository _basketRepository;
        private readonly ClearBasketCommandHandler _handler;

        public ClearBasketCommandHandlerTests()
        {
            _basketRepository = Substitute.For<IBasketRepository>();
            _handler = new ClearBasketCommandHandler(_basketRepository);
        }

        [Fact]
        public async Task Handle_WithExistingBasket_ShouldDeleteAndReturnSuccess()
        {
            var customerId = Guid.NewGuid();
            var basket = Basket.CreateBasket(customerId, Currency.PLN);

            _basketRepository.GetByCustomerId(customerId).Returns(basket);

            var command = new ClearBasketCommand { CustomerId = customerId };

            var result = await _handler.Handle(command);

            Assert.True(result.IsSuccess);
            await _basketRepository.Received(1).Delete(basket);
        }

        [Fact]
        public async Task Handle_WithNonExistentBasket_ShouldReturnBasketNotFound()
        {
            var customerId = Guid.NewGuid();
            _basketRepository.GetByCustomerId(customerId).Returns((Basket)null!);

            var command = new ClearBasketCommand { CustomerId = customerId };

            var result = await _handler.Handle(command);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.BasketNotFound, result.ErrorCode);
        }
    }
}
