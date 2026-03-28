using Bazaar.Baskets.Service.Commands.Handlers.ChangeItemQuantity;
using Bazaar.Baskets.Service.DataAccess.Repository;
using Bazaar.Baskets.Service.Domain.ErrorCodes;
using Bazaar.Baskets.Service.Domain.Models;
using Bazaar.Shared.Models.Models;
using NSubstitute;

namespace Bazaar.Baskets.Service.Tests.Commands
{
    public class ChangeItemQuantityCommandHandlerTests
    {
        private readonly IBasketRepository _basketRepository;
        private readonly ChangeItemQuantityCommandHandler _handler;

        public ChangeItemQuantityCommandHandlerTests()
        {
            _basketRepository = Substitute.For<IBasketRepository>();
            _handler = new ChangeItemQuantityCommandHandler(_basketRepository);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldUpdateQuantityAndReturnSuccess()
        {
            var customerId = Guid.NewGuid();
            var basket = Basket.CreateBasket(customerId, Currency.PLN);
            var item = BasketItem.Create(Guid.NewGuid(), Guid.NewGuid(), basket.Id, "Product", 1, 50m, Currency.PLN, 1m);
            basket.Items.Add(item);

            _basketRepository.GetByCustomerId(customerId).Returns(basket);

            var command = new ChangeItemQuantityCommand
            {
                CustomerId = customerId,
                BasketItemId = item.Id,
                Quantity = 3
            };

            var result = await _handler.Handle(command);

            Assert.True(result.IsSuccess);
            await _basketRepository.Received(1).Update(basket);
        }

        [Fact]
        public async Task Handle_WithZeroQuantity_ShouldReturnValidationFailed()
        {
            var command = new ChangeItemQuantityCommand
            {
                CustomerId = Guid.NewGuid(),
                BasketItemId = Guid.NewGuid(),
                Quantity = 0
            };

            var result = await _handler.Handle(command);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.ValidationFailed, result.ErrorCode);
        }

        [Fact]
        public async Task Handle_WithNegativeQuantity_ShouldReturnValidationFailed()
        {
            var command = new ChangeItemQuantityCommand
            {
                CustomerId = Guid.NewGuid(),
                BasketItemId = Guid.NewGuid(),
                Quantity = -1
            };

            var result = await _handler.Handle(command);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.ValidationFailed, result.ErrorCode);
        }

        [Fact]
        public async Task Handle_WithNonExistentBasket_ShouldReturnBasketNotFound()
        {
            var customerId = Guid.NewGuid();
            _basketRepository.GetByCustomerId(customerId).Returns((Basket)null!);

            var command = new ChangeItemQuantityCommand
            {
                CustomerId = customerId,
                BasketItemId = Guid.NewGuid(),
                Quantity = 2
            };

            var result = await _handler.Handle(command);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.BasketNotFound, result.ErrorCode);
        }

        [Fact]
        public async Task Handle_WithNonExistentItem_ShouldReturnBasketItemNotFound()
        {
            var customerId = Guid.NewGuid();
            var basket = Basket.CreateBasket(customerId, Currency.PLN);

            _basketRepository.GetByCustomerId(customerId).Returns(basket);

            var command = new ChangeItemQuantityCommand
            {
                CustomerId = customerId,
                BasketItemId = Guid.NewGuid(),
                Quantity = 2
            };

            var result = await _handler.Handle(command);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.BasketItemNotFound, result.ErrorCode);
        }
    }
}
