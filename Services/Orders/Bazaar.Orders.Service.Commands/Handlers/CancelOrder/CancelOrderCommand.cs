namespace Bazaar.Orders.Service.Commands.Handlers.CancelOrder
{
    public class CancelOrderCommand
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
    }
}
