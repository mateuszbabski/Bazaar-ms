using Bazaar.Shared.Models.Models;

namespace Bazaar.Baskets.Service.Commands.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
        public string Unit { get; set; }
        public decimal WeightPerUnitInKg { get; set; }
        public bool IsAvailable { get; set; }
    }
}
