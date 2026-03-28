namespace Bazaar.Orders.Service.Commands.Handlers.PayForOrder
{
    public class PayForOrderCommand
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
    }
}
