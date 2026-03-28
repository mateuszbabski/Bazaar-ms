using Bazaar.Orders.Service.Commands.Responses;
using Bazaar.Orders.Service.DataAccess.Repository;
using Bazaar.Orders.Service.Domain.ErrorCodes;
using Bazaar.Orders.Service.Domain.Models;

namespace Bazaar.Orders.Service.Commands.Handlers.PayForOrder
{
    public class PayForOrderCommandHandler
    {
        private readonly IOrderRepository _orderRepository;

        public PayForOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<PayForOrderResult> Handle(PayForOrderCommand command)
        {
            var order = await _orderRepository.GetById(command.OrderId);
            if (order == null)
                return PayForOrderResult.Failure("Order not found.", ErrorCode.OrderNotFound);

            if (order.CustomerId != command.CustomerId)
                return PayForOrderResult.Failure("You are not allowed to pay for this order.", ErrorCode.Forbidden);

            if (order.Status != OrderStatus.Pending)
                return PayForOrderResult.Failure("Only pending orders can be paid.", ErrorCode.InvalidOrderStatus);

            order.Pay();
            await _orderRepository.Update(order);

            return PayForOrderResult.Success("Order has been paid.");
        }
    }
}
