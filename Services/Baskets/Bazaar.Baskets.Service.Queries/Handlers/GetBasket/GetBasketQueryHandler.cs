using Bazaar.Baskets.Service.DataAccess.Repository;
using Bazaar.Baskets.Service.Domain.ErrorCodes;
using Bazaar.Baskets.Service.Queries.Dtos;
using Bazaar.Baskets.Service.Queries.Responses;

namespace Bazaar.Baskets.Service.Queries.Handlers.GetBasket
{
    public class GetBasketQueryHandler
    {
        private readonly IBasketRepository _basketRepository;

        public GetBasketQueryHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<GetBasketResult> Handle(GetBasketQuery query)
        {
            var basket = await _basketRepository.GetByCustomerId(query.CustomerId);
            if (basket == null)
                return GetBasketResult.Failure("Basket not found.", ErrorCode.BasketNotFound);

            var basketDto = new BasketDto
            {
                Id = basket.Id,
                CustomerId = basket.CustomerId,
                TotalPrice = basket.TotalPrice,
                Currency = basket.Currency,
                TotalWeightInKg = basket.TotalWeightInKg,
                Items = basket.Items.Select(i => new BasketItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ShopId = i.ShopId,
                    Name = i.Name,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Currency = i.Currency,
                    BasePrice = i.BasePrice,
                    BaseCurrency = i.BaseCurrency,
                    WeightInKg = i.WeightInKg
                }).ToList()
            };

            return GetBasketResult.Success(basketDto, "Basket retrieved successfully.");
        }
    }
}
