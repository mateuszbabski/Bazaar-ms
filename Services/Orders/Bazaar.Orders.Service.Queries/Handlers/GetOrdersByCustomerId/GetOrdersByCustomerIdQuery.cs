using Bazaar.Orders.Service.Domain.Models;

namespace Bazaar.Orders.Service.Queries.Handlers.GetOrdersByCustomerId
{
    public class GetOrdersByCustomerIdQuery
    {
        public Guid CustomerId { get; set; }
        public OrderStatus? Status { get; set; }
    }
}
