using Bazaar.Baskets.Service.DataAccess.Repository;
using Bazaar.Baskets.Service.Domain.ErrorCodes;
using Bazaar.Baskets.Service.Domain.Models;
using Bazaar.Baskets.Service.Queries.Handlers.GetBasket;
using Bazaar.Shared.Models.Models;
using NSubstitute;

namespace Bazaar.Baskets.Service.Tests.Queries
{
    public class GetBasketQueryHandlerTests
    {
        private readonly IBasketRepository _basketRepository;
        private readonly GetBasketQueryHandler _handler;

        public GetBasketQueryHandlerTests()
        {
            _basketRepository = Substitute.For<IBasketRepository>();
            _handler = new GetBasketQueryHandler(_basketRepository);
        }

        [Fact]
        public async Task Handle_WithExistingBasket_ShouldReturnSuccess()
        {
            var customerId = Guid.NewGuid();
            var basket = Basket.CreateBasket(customerId, Currency.PLN);
            var item = BasketItem.Create(Guid.NewGuid(), Guid.NewGuid(), basket.Id, "Product", 2, 50m, Currency.PLN, 1.5m);
            basket.Items.Add(item);

            _basketRepository.GetByCustomerId(customerId).Returns(basket);

            var query = new GetBasketQuery { CustomerId = customerId };

            var result = await _handler.Handle(query);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Basket);
            Assert.Equal(customerId, result.Basket.CustomerId);
            Assert.Single(result.Basket.Items);
        }

        [Fact]
        public async Task Handle_WithNonExistentBasket_ShouldReturnBasketNotFound()
        {
            var customerId = Guid.NewGuid();
            _basketRepository.GetByCustomerId(customerId).Returns((Basket)null!);

            var query = new GetBasketQuery { CustomerId = customerId };

            var result = await _handler.Handle(query);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.BasketNotFound, result.ErrorCode);
        }
    }
}
