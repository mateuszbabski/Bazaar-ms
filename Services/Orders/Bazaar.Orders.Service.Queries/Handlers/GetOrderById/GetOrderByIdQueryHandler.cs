using Bazaar.Orders.Service.DataAccess.Repository;
using Bazaar.Orders.Service.Domain.ErrorCodes;
using Bazaar.Orders.Service.Queries.Dtos;
using Bazaar.Orders.Service.Queries.Responses;

namespace Bazaar.Orders.Service.Queries.Handlers.GetOrderById
{
    public class GetOrderByIdQueryHandler
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<GetOrderByIdResult> Handle(GetOrderByIdQuery query)
        {
            var order = await _orderRepository.GetById(query.OrderId);
            if (order == null)
                return GetOrderByIdResult.Failure("Order not found.", ErrorCode.OrderNotFound);

            if (order.CustomerId != query.CustomerId)
                return GetOrderByIdResult.Failure("You are not allowed to view this order.", ErrorCode.Forbidden);

            var orderDto = new OrderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                TotalPrice = order.TotalPrice,
                Currency = order.Currency,
                TotalWeightInKg = order.TotalWeightInKg,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ShopId = i.ShopId,
                    Name = i.Name,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Currency = i.Currency,
                    WeightInKg = i.WeightInKg
                }).ToList()
            };

            return GetOrderByIdResult.Success(orderDto, "Order retrieved successfully.");
        }
    }
}
