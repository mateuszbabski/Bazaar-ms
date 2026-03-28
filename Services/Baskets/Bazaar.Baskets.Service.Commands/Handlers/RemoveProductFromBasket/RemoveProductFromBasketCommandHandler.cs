using Bazaar.Baskets.Service.Commands.Responses;
using Bazaar.Baskets.Service.DataAccess.Repository;
using Bazaar.Baskets.Service.Domain.ErrorCodes;

namespace Bazaar.Baskets.Service.Commands.Handlers.RemoveProductFromBasket
{
    public class RemoveProductFromBasketCommandHandler
    {
        private readonly IBasketRepository _basketRepository;

        public RemoveProductFromBasketCommandHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<RemoveProductFromBasketResult> Handle(RemoveProductFromBasketCommand command)
        {
            var basket = await _basketRepository.GetByCustomerId(command.CustomerId);
            if (basket == null)
                return RemoveProductFromBasketResult.Failure("Basket not found.", ErrorCode.BasketNotFound);

            var item = basket.Items.FirstOrDefault(i => i.Id == command.BasketItemId);
            if (item == null)
                return RemoveProductFromBasketResult.Failure("Item not found in basket.", ErrorCode.BasketItemNotFound);

            basket.RemoveItem(command.BasketItemId);

            if (basket.Items.Count == 0)
                await _basketRepository.Delete(basket);
            else
                await _basketRepository.Update(basket);

            return RemoveProductFromBasketResult.Success("Product removed from basket successfully.");
        }
    }
}
