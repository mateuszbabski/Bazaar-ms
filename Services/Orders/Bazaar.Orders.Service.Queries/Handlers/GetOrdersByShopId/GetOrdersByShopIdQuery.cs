using Bazaar.Orders.Service.Domain.Models;

namespace Bazaar.Orders.Service.Queries.Handlers.GetOrdersByShopId
{
    public class GetOrdersByShopIdQuery
    {
        public Guid ShopId { get; set; }
        public OrderStatus? Status { get; set; }
    }
}
