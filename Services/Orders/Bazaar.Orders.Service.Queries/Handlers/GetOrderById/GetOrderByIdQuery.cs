namespace Bazaar.Orders.Service.Queries.Handlers.GetOrderById
{
    public class GetOrderByIdQuery
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
    }
}
