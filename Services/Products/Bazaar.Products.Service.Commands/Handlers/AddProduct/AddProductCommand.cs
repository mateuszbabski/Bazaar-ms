namespace Bazaar.Products.Service.Commands.Handlers.AddProduct
{
    public class AddProductCommand
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string Unit { get; set; }
        public decimal WeightPerUnitInKg { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
