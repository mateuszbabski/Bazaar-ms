using Bazaar.Shared.Models.Models;

namespace Bazaar.Baskets.Service.Queries.Dtos
{
    public class BasketDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public Currency Currency { get; set; }
        public decimal TotalWeightInKg { get; set; }
        public List<BasketItemDto> Items { get; set; } = [];
    }

    public class BasketItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid ShopId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
        public decimal BasePrice { get; set; }
        public Currency BaseCurrency { get; set; }
        public decimal WeightInKg { get; set; }
    }
}
