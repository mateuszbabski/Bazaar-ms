using Bazaar.Baskets.Service.Commands.Responses;
using Bazaar.Baskets.Service.DataAccess.Repository;
using Bazaar.Baskets.Service.Domain.ErrorCodes;

namespace Bazaar.Baskets.Service.Commands.Handlers.ClearBasket
{
    public class ClearBasketCommandHandler
    {
        private readonly IBasketRepository _basketRepository;

        public ClearBasketCommandHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<ClearBasketResult> Handle(ClearBasketCommand command)
        {
            var basket = await _basketRepository.GetByCustomerId(command.CustomerId);
            if (basket == null)
                return ClearBasketResult.Failure("Basket not found.", ErrorCode.BasketNotFound);

            await _basketRepository.Delete(basket);

            return ClearBasketResult.Success("Basket cleared successfully.");
        }
    }
}
