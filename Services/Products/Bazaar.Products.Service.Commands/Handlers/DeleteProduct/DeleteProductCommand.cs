namespace Bazaar.Products.Service.Commands.Handlers.DeleteProduct
{
    public class DeleteProductCommand
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
    }
}
