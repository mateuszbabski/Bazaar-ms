using Bazaar.Orders.Service.Commands.Responses;
using Bazaar.Orders.Service.DataAccess.Repository;
using Bazaar.Orders.Service.Domain.ErrorCodes;
using Bazaar.Orders.Service.Domain.Models;
using Bazaar.Shared.Infrastructure.TimeProvider;

namespace Bazaar.Orders.Service.Commands.Handlers.PlaceOrder
{
    public class PlaceOrderCommandHandler
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public PlaceOrderCommandHandler(
            IOrderRepository orderRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _orderRepository = orderRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<PlaceOrderResult> Handle(PlaceOrderCommand command)
        {
            if (command.Items.Count == 0)
                return PlaceOrderResult.Failure("Order must contain at least one item.", ErrorCode.ValidationFailed);

            var order = Order.Create(
                command.CustomerId,
                command.TotalPrice,
                command.Currency,
                command.TotalWeightInKg,
                _dateTimeProvider.UtcNow);

            foreach (var item in command.Items)
            {
                var orderItem = OrderItem.Create(
                    order.Id,
                    item.ProductId,
                    item.ShopId,
                    item.Name,
                    item.Quantity,
                    item.Price,
                    item.Currency,
                    item.WeightInKg);

                order.Items.Add(orderItem);
            }

            await _orderRepository.Add(order);

            return PlaceOrderResult.Success("Order has been placed.", order.Id);
        }
    }
}
