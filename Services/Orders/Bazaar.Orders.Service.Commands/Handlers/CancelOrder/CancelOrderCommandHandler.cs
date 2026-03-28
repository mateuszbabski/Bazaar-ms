using Bazaar.Orders.Service.Commands.Responses;
using Bazaar.Orders.Service.DataAccess.Repository;
using Bazaar.Orders.Service.Domain.ErrorCodes;
using Bazaar.Orders.Service.Domain.Models;

namespace Bazaar.Orders.Service.Commands.Handlers.CancelOrder
{
    public class CancelOrderCommandHandler
    {
        private readonly IOrderRepository _orderRepository;

        public CancelOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<CancelOrderResult> Handle(CancelOrderCommand command)
        {
            var order = await _orderRepository.GetById(command.OrderId);
            if (order == null)
                return CancelOrderResult.Failure("Order not found.", ErrorCode.OrderNotFound);

            if (order.CustomerId != command.CustomerId)
                return CancelOrderResult.Failure("You are not allowed to cancel this order.", ErrorCode.Forbidden);

            if (order.Status != OrderStatus.Pending)
                return CancelOrderResult.Failure("Only pending orders can be cancelled.", ErrorCode.InvalidOrderStatus);

            order.Cancel();
            await _orderRepository.Update(order);

            return CancelOrderResult.Success("Order has been cancelled.");
        }
    }
}
