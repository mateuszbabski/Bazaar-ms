using Bazaar.Baskets.Service.Commands.Handlers.RemoveProductFromBasket;
using Bazaar.Baskets.Service.DataAccess.Repository;
using Bazaar.Baskets.Service.Domain.ErrorCodes;
using Bazaar.Baskets.Service.Domain.Models;
using Bazaar.Shared.Models.Models;
using NSubstitute;

namespace Bazaar.Baskets.Service.Tests.Commands
{
    public class RemoveProductFromBasketCommandHandlerTests
    {
        private readonly IBasketRepository _basketRepository;
        private readonly RemoveProductFromBasketCommandHandler _handler;

        public RemoveProductFromBasketCommandHandlerTests()
        {
            _basketRepository = Substitute.For<IBasketRepository>();
            _handler = new RemoveProductFromBasketCommandHandler(_basketRepository);
        }

        [Fact]
        public async Task Handle_WithValidItem_ShouldRemoveAndReturnSuccess()
        {
            var customerId = Guid.NewGuid();
            var basket = Basket.CreateBasket(customerId, Currency.PLN);
            var item = BasketItem.Create(Guid.NewGuid(), Guid.NewGuid(), basket.Id, "Product", 1, 50m, Currency.PLN, 1m);
            var secondItem = BasketItem.Create(Guid.NewGuid(), Guid.NewGuid(), basket.Id, "Product 2", 1, 30m, Currency.PLN, 1m);
            basket.Items.Add(item);
            basket.Items.Add(secondItem);

            _basketRepository.GetByCustomerId(customerId).Returns(basket);

            var command = new RemoveProductFromBasketCommand { CustomerId = customerId, BasketItemId = item.Id };

            var result = await _handler.Handle(command);

            Assert.True(result.IsSuccess);
            await _basketRepository.Received(1).Update(basket);
        }

        [Fact]
        public async Task Handle_WithLastItem_ShouldDeleteBasket()
        {
            var customerId = Guid.NewGuid();
            var basket = Basket.CreateBasket(customerId, Currency.PLN);
            var item = BasketItem.Create(Guid.NewGuid(), Guid.NewGuid(), basket.Id, "Product", 1, 50m, Currency.PLN, 1m);
            basket.Items.Add(item);

            _basketRepository.GetByCustomerId(customerId).Returns(basket);

            var command = new RemoveProductFromBasketCommand { CustomerId = customerId, BasketItemId = item.Id };

            var result = await _handler.Handle(command);

            Assert.True(result.IsSuccess);
            await _basketRepository.Received(1).Delete(basket);
        }

        [Fact]
        public async Task Handle_WithNonExistentBasket_ShouldReturnBasketNotFound()
        {
            var customerId = Guid.NewGuid();
            _basketRepository.GetByCustomerId(customerId).Returns((Basket)null!);

            var command = new RemoveProductFromBasketCommand { CustomerId = customerId, BasketItemId = Guid.NewGuid() };

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

            var command = new RemoveProductFromBasketCommand { CustomerId = customerId, BasketItemId = Guid.NewGuid() };

            var result = await _handler.Handle(command);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.BasketItemNotFound, result.ErrorCode);
        }
    }
}
