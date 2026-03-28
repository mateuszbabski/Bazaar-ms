using Bazaar.Baskets.Service.Commands.Clients;
using Bazaar.Baskets.Service.Commands.Responses;
using Bazaar.Baskets.Service.DataAccess.Repository;
using Bazaar.Baskets.Service.Domain.ErrorCodes;

namespace Bazaar.Baskets.Service.Commands.Handlers.CheckoutBasket
{
    public class CheckoutBasketCommandHandler
    {
        private readonly IBasketRepository _basketRepository;
        private readonly OrderServiceClient _orderServiceClient;

        public CheckoutBasketCommandHandler(
            IBasketRepository basketRepository,
            OrderServiceClient orderServiceClient)
        {
            _basketRepository = basketRepository;
            _orderServiceClient = orderServiceClient;
        }

        public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command)
        {
            var basket = await _basketRepository.GetByCustomerId(command.CustomerId);
            if (basket == null)
                return CheckoutBasketResult.Failure("Basket not found.", ErrorCode.BasketNotFound);

            if (basket.Items.Count == 0)
                return CheckoutBasketResult.Failure("Cannot checkout an empty basket.", ErrorCode.BasketEmpty);

            var orderResult = await _orderServiceClient.PlaceOrder(basket);
            if (!orderResult.IsSuccess)
                return CheckoutBasketResult.Failure(orderResult.ErrorMessage!, ErrorCode.UnknownError);

            await _basketRepository.Delete(basket);

            return CheckoutBasketResult.Success("Checkout completed. Order has been placed.");
        }
    }
}
