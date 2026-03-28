using Bazaar.Baskets.Service.Commands.Responses;
using Bazaar.Baskets.Service.DataAccess.Repository;
using Bazaar.Baskets.Service.Domain.ErrorCodes;

namespace Bazaar.Baskets.Service.Commands.Handlers.ChangeItemQuantity
{
    public class ChangeItemQuantityCommandHandler
    {
        private readonly IBasketRepository _basketRepository;

        public ChangeItemQuantityCommandHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<ChangeItemQuantityResult> Handle(ChangeItemQuantityCommand command)
        {
            if (command.Quantity <= 0)
                return ChangeItemQuantityResult.Failure("Quantity must be greater than zero.", ErrorCode.ValidationFailed);

            var basket = await _basketRepository.GetByCustomerId(command.CustomerId);
            if (basket == null)
                return ChangeItemQuantityResult.Failure("Basket not found.", ErrorCode.BasketNotFound);

            var item = basket.Items.FirstOrDefault(i => i.Id == command.BasketItemId);
            if (item == null)
                return ChangeItemQuantityResult.Failure("Item not found in basket.", ErrorCode.BasketItemNotFound);

            item.ChangeBasketItemQuantity(command.Quantity);
            basket.CountTotalPrice();
            basket.CalculateTotalWeight();

            await _basketRepository.Update(basket);

            return ChangeItemQuantityResult.Success("Item quantity updated successfully.");
        }
    }
}
