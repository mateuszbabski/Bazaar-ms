using Bazaar.Orders.Service.DataAccess.Repository;
using Bazaar.Orders.Service.Domain.ErrorCodes;
using Bazaar.Orders.Service.Queries.Dtos;
using Bazaar.Orders.Service.Queries.Responses;

namespace Bazaar.Orders.Service.Queries.Handlers.GetOrdersByCustomerId
{
    public class GetOrdersByCustomerIdQueryHandler
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrdersByCustomerIdQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<GetOrdersResult> Handle(GetOrdersByCustomerIdQuery query)
        {
            var orders = await _orderRepository.GetByCustomerId(query.CustomerId, query.Status);
            if (!orders.Any())
                return GetOrdersResult.Failure("No orders found.", ErrorCode.OrdersNotFound);

            var orderDtos = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                TotalPrice = o.TotalPrice,
                Currency = o.Currency,
                TotalWeightInKg = o.TotalWeightInKg,
                Status = o.Status,
                CreatedAt = o.CreatedAt,
                Items = o.Items.Select(i => new OrderItemDto
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
            }).ToList();

            return GetOrdersResult.Success(orderDtos, "Orders retrieved successfully.");
        }
    }
}
