using Bazaar.Products.Service.Domain.Models;

namespace Bazaar.Products.Service.DataAccess.Repository
{
    public interface IProductRepository
    {
        Task<Product> Add(Product product);
        Task<Product> GetById(Guid id);
        Task<IEnumerable<Product>> GetAllProducts();
        Task<IEnumerable<Product>> GetProductsByShopId(Guid shopId);
        Task<IEnumerable<Product>> SearchProducts(string? name, string? category, decimal? minPrice, decimal? maxPrice);
        Task Update(Product product);
        Task Delete(Product product);
    }
}
