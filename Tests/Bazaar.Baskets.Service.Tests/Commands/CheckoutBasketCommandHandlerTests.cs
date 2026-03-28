using Bazaar.Baskets.Service.Commands.Handlers.CheckoutBasket;
using Bazaar.Baskets.Service.DataAccess.Repository;
using Bazaar.Baskets.Service.Domain.ErrorCodes;
using Bazaar.Baskets.Service.Domain.Models;
using Bazaar.Shared.Models.Models;
using NSubstitute;
using System.Reflection;

namespace Bazaar.Baskets.Service.Tests.Commands
{
    public class CheckoutBasketCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WithNonExistentBasket_ShouldReturnBasketNotFound()
        {
            var basketRepository = Substitute.For<IBasketRepository>();
            var customerId = Guid.NewGuid();
            basketRepository.GetByCustomerId(customerId).Returns((Basket)null!);

            var handler = CreateHandlerWithRepository(basketRepository);
            var command = new CheckoutBasketCommand { CustomerId = customerId };

            var result = await handler.Handle(command);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.BasketNotFound, result.ErrorCode);
        }

        [Fact]
        public async Task Handle_WithEmptyBasket_ShouldReturnBasketEmpty()
        {
            var basketRepository = Substitute.For<IBasketRepository>();
            var customerId = Guid.NewGuid();
            var basket = Basket.CreateBasket(customerId, Currency.PLN);

            basketRepository.GetByCustomerId(customerId).Returns(basket);

            var handler = CreateHandlerWithRepository(basketRepository);
            var command = new CheckoutBasketCommand { CustomerId = customerId };

            var result = await handler.Handle(command);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.BasketEmpty, result.ErrorCode);
        }

        /// <summary>
        /// Creates a handler with only the basket repository set.
        /// The tested paths exit before reaching OrderServiceClient (concrete HTTP class).
        /// Full checkout flow requires integration tests.
        /// </summary>
        private static CheckoutBasketCommandHandler CreateHandlerWithRepository(IBasketRepository basketRepository)
        {
#pragma warning disable SYSLIB0050
            var handler = (CheckoutBasketCommandHandler)System.Runtime.Serialization.FormatterServices
                .GetUninitializedObject(typeof(CheckoutBasketCommandHandler));
#pragma warning restore SYSLIB0050

            typeof(CheckoutBasketCommandHandler)
                .GetField("_basketRepository", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(handler, basketRepository);

            return handler;
        }
    }
}
